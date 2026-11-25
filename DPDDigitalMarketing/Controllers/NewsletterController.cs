using Microsoft.AspNetCore.Mvc;
using DPDDigitalMarketing.Services;

namespace DPDDigitalMarketing.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly IEmailMarketingService _emailService;

        public NewsletterController(IEmailMarketingService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string email, string? fullName, string preference = "All", string returnUrl = "/")
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ErrorMessage"] = "Please enter a valid email address.";
                return LocalRedirect(returnUrl);
            }

            var success = await _emailService.SubscribeAsync(email, fullName ?? "", preference);
            if (success)
            {
                TempData["SuccessMessage"] = "🎉 Subscribed successfully! You will receive new issue and job alerts.";
            }

            return LocalRedirect(returnUrl);
        }
    }
}
