using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Models;

namespace DPDDigitalMarketing.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? category, string? search, string? language)
        {
            var query = _context.Products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(p => p.Language == language);
            }

            if (!string.IsNullOrEmpty(search))
            {
                var term = search.Trim();
                query = query.Where(p =>
                    EF.Functions.Like(p.Title, $"%{term}%") ||
                    EF.Functions.Like(p.Description, $"%{term}%") ||
                    (p.MetaKeywords != null && EF.Functions.Like(p.MetaKeywords, $"%{term}%")));
            }

            ViewBag.SelectedCategory = category;
            ViewBag.SearchTerm = search;
            ViewBag.SelectedLanguage = language;

            var products = await query.OrderByDescending(p => p.IssueDate).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.UtcNow;
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Publication added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Publication description and details updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == product.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Publication archived.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
