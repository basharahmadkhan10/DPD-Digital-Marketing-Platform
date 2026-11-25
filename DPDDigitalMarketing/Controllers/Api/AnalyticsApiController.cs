using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Services;

namespace DPDDigitalMarketing.Controllers.Api
{

    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsApiController(AppDbContext context, IAnalyticsService analyticsService)
        {
            _context = context;
            _analyticsService = analyticsService;
        }

        [HttpGet("channels")]
        public async Task<IActionResult> GetChannelDistribution()
        {
            var data = await _analyticsService.GetSalesByChannelAsync();

            var result = new
            {
                labels = data.Select(d => d.Channel == "Website" ? "Website (Direct)" : $"{d.Channel} (Referral)").ToList(),
                revenues = data.Select(d => d.TotalRevenue).ToList(),
                attributedRevenues = data.Select(d => d.AttributedRevenue).ToList(),
                orderCounts = data.Select(d => d.OrderCount).ToList(),
                channelTypes = data.Select(d => d.ChannelType).ToList()
            };

            return Ok(result);
        }

        [HttpGet("campaign-roi")]
        public async Task<IActionResult> GetCampaignROI()
        {
            var campaigns = await _context.Campaigns.ToListAsync();
            var result = campaigns.Select(c => new
            {
                c.CampaignName,
                c.Channel,
                c.Cost,
                c.RevenueGenerated,
                c.Conversions,
                ROI = c.ROI
            });

            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _analyticsService.GetDashboardSummaryAsync();
            return Ok(summary);
        }
    }
}
