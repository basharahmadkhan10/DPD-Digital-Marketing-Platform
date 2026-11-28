using Microsoft.AspNetCore.Mvc;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Services;

namespace DPDDigitalMarketing.Controllers
{

    public class LibraryController : Controller
    {
        private readonly IDigitalLibraryService _libraryService;
        private readonly AppDbContext _context;

        public LibraryController(IDigitalLibraryService libraryService, AppDbContext context)
        {
            _libraryService = libraryService;
            _context = context;
        }

        public IActionResult Index()
        {
            var issues = _libraryService.GetAllDigitalIssues();
            var userEmail = User.Identity?.Name ?? HttpContext.Session.GetString("Customer_Email");

            var unlockedIds = new HashSet<int>();
            var isAdmin = User.IsInRole("Admin");
            foreach (var issue in issues)
            {
                if (isAdmin || LibraryAccessHelper.IsProductUnlocked(HttpContext.Session, issue.ProductId, userEmail, _context))
                {
                    unlockedIds.Add(issue.ProductId);
                }
            }

            ViewBag.UnlockedIds = unlockedIds;
            return View(issues);
        }

        public IActionResult Read(int id, string? section = null)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var issue = _libraryService.GetDigitalIssue(id);
            if (issue == null)
            {
                return NotFound();
            }

            var userEmail = User.Identity?.Name ?? HttpContext.Session.GetString("Customer_Email");
            var isUnlocked = User.IsInRole("Admin") || LibraryAccessHelper.IsProductUnlocked(HttpContext.Session, id, userEmail, _context);

            var activeSection = issue.Sections.FirstOrDefault(s => s.Id == section)
                                ?? issue.Sections.FirstOrDefault();

            ViewBag.IsUnlocked = isUnlocked;
            ViewBag.ProductPrice = product.Price;
            ViewBag.ActiveSection = activeSection;
            ViewBag.SectionIndex = issue.Sections.IndexOf(activeSection ?? new IssueSection()) + 1;
            ViewBag.TotalSections = issue.Sections.Count;

            return View(issue);
        }

        public IActionResult DownloadPdf(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            var issue = _libraryService.GetDigitalIssue(id);
            if (issue == null) return NotFound();

            var userEmail = User.Identity?.Name ?? HttpContext.Session.GetString("Customer_Email");
            var isUnlocked = User.IsInRole("Admin") || LibraryAccessHelper.IsProductUnlocked(HttpContext.Session, id, userEmail, _context);

            if (!isUnlocked)
            {
                TempData["ErrorMessage"] = "Please unlock or purchase this publication to download its official PDF edition.";
                return RedirectToAction(nameof(Read), new { id });
            }

            var pdfBytes = PdfDocumentBuilder.BuildIssuePdf(issue, userEmail);
            var safeName = string.Concat(issue.Title.Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-')).Trim().Replace(' ', '_');
            if (string.IsNullOrWhiteSpace(safeName)) safeName = $"DPD_Publication_{id}";

            return File(pdfBytes, "application/pdf", $"{safeName}.pdf");
        }
    }
}
