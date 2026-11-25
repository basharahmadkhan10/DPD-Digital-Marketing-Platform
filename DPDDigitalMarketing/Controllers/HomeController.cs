using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Models;

namespace DPDDigitalMarketing.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        var employmentNews = await _context.Products
            .Where(p => p.IsActive && p.Category == "EmploymentNews")
            .OrderByDescending(p => p.IssueDate)
            .Take(3)
            .ToListAsync();

        var journals = await _context.Products
            .Where(p => p.IsActive && p.Category == "Journal")
            .OrderByDescending(p => p.IssueDate)
            .Take(4)
            .ToListAsync();

        var p1 = await _context.Products.FindAsync(1);
        var p3 = await _context.Products.FindAsync(3);
        var p4 = await _context.Products.FindAsync(4);
        var curatedTrio = new List<Product>();
        if (p1 != null) curatedTrio.Add(p1);
        if (p3 != null) curatedTrio.Add(p3);
        if (p4 != null) curatedTrio.Add(p4);

        ViewBag.EmploymentNews = employmentNews;
        ViewBag.Journals = journals;
        ViewBag.CuratedTrio = curatedTrio.Any() ? curatedTrio : employmentNews.Concat(journals).Take(3).ToList();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
