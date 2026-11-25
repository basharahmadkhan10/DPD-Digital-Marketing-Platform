using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Models;
using DPDDigitalMarketing.Services;

namespace DPDDigitalMarketing.Controllers
{
    public class MarketingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAnalyticsService _analyticsService;
        private readonly IEmailMarketingService _emailService;

        public MarketingController(
            AppDbContext context,
            IAnalyticsService analyticsService,
            IEmailMarketingService emailService)
        {
            _context = context;
            _analyticsService = analyticsService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var summary = await _analyticsService.GetDashboardSummaryAsync();
            var campaigns = await _context.Campaigns.OrderByDescending(c => c.StartDate).ToListAsync();
            ViewBag.Campaigns = campaigns;
            return View(summary);
        }

        public async Task<IActionResult> Campaigns()
        {
            var campaigns = await _context.Campaigns.OrderByDescending(c => c.StartDate).ToListAsync();
            return View(campaigns);
        }

        public IActionResult CreateCampaign()
        {
            return View(new Campaign());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCampaign(Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                campaign.StartDate = DateTime.UtcNow;
                campaign.Status = "Active";
                _context.Campaigns.Add(campaign);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Campaign '{campaign.CampaignName}' created successfully!";
                return RedirectToAction(nameof(Campaigns));
            }
            return View(campaign);
        }

        public async Task<IActionResult> Subscribers()
        {
            var list = await _context.Subscribers.OrderByDescending(s => s.SubscribedAt).ToListAsync();
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailBlast(string subject, string messageContent, string targetGroup)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                TempData["ErrorMessage"] = "Subject line cannot be empty.";
                return RedirectToAction(nameof(Dashboard));
            }

            int count = await _emailService.SendJobAlertBlastAsync(subject, messageContent ?? "", targetGroup ?? "All");
            TempData["SuccessMessage"] = $"Email broadcast sent successfully to {count} subscribers!";
            return RedirectToAction(nameof(Dashboard));
        }
    }
}
