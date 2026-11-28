using Microsoft.AspNetCore.Mvc;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Models;
using DPDDigitalMarketing.Services;

namespace DPDDigitalMarketing.Controllers
{

    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Admin accounts are for management only and cannot make purchases.";
                return RedirectToAction("Dashboard", "Marketing");
            }

            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1, string? returnUrl = null)
        {
            if (User.IsInRole("Admin"))
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    Request.Headers.Accept.ToString().Contains("application/json") ||
                    Request.ContentType?.Contains("application/json") == true)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Admin accounts are for management only and cannot add items to bag."
                    });
                }
                TempData["ErrorMessage"] = "Admin accounts are for management only and cannot make purchases.";
                return RedirectToAction("Dashboard", "Marketing");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += Math.Max(1, quantity);
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Quantity = Math.Max(1, quantity),
                    CoverImageUrl = product.CoverImageUrl,
                    Category = product.Category,
                    Language = product.Language
                });
            }

            CartSessionHelper.SaveCart(HttpContext.Session, cart);
            TempData["SuccessMessage"] = $"Added '{product.Title}' to your reading bag.";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                Request.Headers.Accept.ToString().Contains("application/json") ||
                Request.ContentType?.Contains("application/json") == true)
            {
                var totalCount = CartSessionHelper.GetTotalCount(HttpContext.Session);
                return Json(new
                {
                    success = true,
                    cartCount = totalCount,
                    title = product.Title,
                    price = product.Price,
                    message = $"'{product.Title}' added to your reading bag."
                });
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int delta)
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                item.Quantity += delta;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
                CartSessionHelper.SaveCart(HttpContext.Session, cart);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                CartSessionHelper.SaveCart(HttpContext.Session, cart);
                TempData["SuccessMessage"] = "Item removed from your bag.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetCount()
        {
            var count = CartSessionHelper.GetTotalCount(HttpContext.Session);
            return Json(new { count });
        }
    }
}
