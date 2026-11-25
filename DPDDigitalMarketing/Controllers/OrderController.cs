using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Models;
using DPDDigitalMarketing.Services;

namespace DPDDigitalMarketing.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailMarketingService _emailService;

        public OrderController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IEmailMarketingService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        [Authorize]
        public async Task<IActionResult> Checkout(int? productId = null, string channel = "Website")
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);

            if (productId.HasValue && !cart.Any(c => c.ProductId == productId.Value))
            {
                var prod = await _context.Products.FindAsync(productId.Value);
                if (prod != null && prod.IsActive)
                {
                    cart.Add(new CartItem
                    {
                        ProductId = prod.Id,
                        Title = prod.Title,
                        Price = prod.Price,
                        Quantity = 1,
                        CoverImageUrl = prod.CoverImageUrl,
                        Category = prod.Category,
                        Language = prod.Language
                    });
                    CartSessionHelper.SaveCart(HttpContext.Session, cart);
                }
            }

            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Your shopping bag is empty. Please add publications to checkout.";
                return RedirectToAction("Index", "Cart");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            ViewBag.CartItems = cart;
            ViewBag.Channel = channel;
            ViewBag.TotalAmount = cart.Sum(i => i.Total);
            ViewBag.CurrentUser = currentUser;

            var order = new Order
            {
                TotalAmount = cart.Sum(i => i.Total),
                SaleChannel = channel,
                CustomerName = currentUser?.FullName ?? User.Identity?.Name ?? "",
                CustomerEmail = currentUser?.Email ?? User.Identity?.Name ?? ""
            };

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Your shopping bag is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                order.CustomerEmail = currentUser.Email!;
                if (string.IsNullOrEmpty(order.CustomerName)) order.CustomerName = currentUser.FullName ?? currentUser.UserName!;
            }

            order.OrderDate = DateTime.UtcNow;
            order.Status = "Completed";
            order.TotalAmount = cart.Sum(i => i.Total);
            order.SaleChannel = string.IsNullOrEmpty(order.SaleChannel) ? "Website" : order.SaleChannel;

            foreach (var item in cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var purchasedProductIds = cart.Select(i => i.ProductId).ToList();
            LibraryAccessHelper.UnlockProducts(HttpContext.Session, purchasedProductIds);
            if (!string.IsNullOrEmpty(order.CustomerEmail))
            {
                HttpContext.Session.SetString("Customer_Email", order.CustomerEmail);
            }

            CartSessionHelper.ClearCart(HttpContext.Session);

            await _emailService.SendOrderConfirmationEmailAsync(order, cart);

            TempData["SuccessMessage"] = $"Thank you! Order #{order.Id} placed successfully via {order.SaleChannel}. Confirmation email sent!";
            return RedirectToAction(nameof(Confirmation), new { id = order.Id });
        }

        public async Task<IActionResult> TrackOutboundSale(int productId, string channel)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var order = new Order
            {
                CustomerName = $"Marketplace Lead ({channel})",
                CustomerEmail = $"outbound-{channel.ToLower()}@dpd.gov.in",
                SaleChannel = channel,
                TotalAmount = 0.00m,
                OrderDate = DateTime.UtcNow,
                Status = "Outbound Referral"
            };

            order.OrderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = 1,
                UnitPrice = product.Price
            });

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            if (channel == "Amazon" && !string.IsNullOrEmpty(product.AmazonUrl))
            {
                return Redirect(product.AmazonUrl);
            }
            if (channel == "Flipkart" && !string.IsNullOrEmpty(product.FlipkartUrl))
            {
                return Redirect(product.FlipkartUrl);
            }

            return RedirectToAction("Details", "Product", new { id = productId });
        }

        [Authorize]
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");
            var isOwner = currentUser?.Email != null &&
                          string.Equals(order.CustomerEmail, currentUser.Email, StringComparison.OrdinalIgnoreCase);

            if (!isAdmin && !isOwner)
            {
                return Forbid();
            }

            return View(order);
        }

        [Authorize]
        public async Task<IActionResult> SalesHistory()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isStaff = User.IsInRole("Admin");

            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();

            if (!isStaff && currentUser != null)
            {
                var userEmail = (currentUser.Email ?? "").ToLower();
                query = query.Where(o => o.CustomerEmail.ToLower() == userEmail);
            }

            var orders = await query.ToListAsync();
            ViewBag.IsStaff = isStaff;
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ClearAllOrders()
        {
            _context.OrderItems.RemoveRange(_context.OrderItems);
            _context.Orders.RemoveRange(_context.Orders);
            await _context.SaveChangesAsync();

            CartSessionHelper.ClearCart(HttpContext.Session);
            HttpContext.Session.Remove("DPD_Unlocked_Products");
            HttpContext.Session.Remove("Customer_Email");

            TempData["SuccessMessage"] = "All orders, marketplace leads, and cart items cleared. Database reset to clean state!";
            return RedirectToAction(nameof(SalesHistory));
        }
    }
}
