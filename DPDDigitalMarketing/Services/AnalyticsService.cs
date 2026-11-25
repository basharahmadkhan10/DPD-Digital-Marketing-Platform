using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Data;

namespace DPDDigitalMarketing.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly AppDbContext _context;

        public AnalyticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChannelSalesSummary>> GetSalesByChannelAsync()
        {

            var ordersWithItems = await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();

            var channelData = ordersWithItems
                .GroupBy(o => o.SaleChannel)
                .Select(g => new ChannelSalesSummary
                {
                    Channel = g.Key,

                    TotalRevenue = g.Where(o => o.Status == "Completed").Sum(o => o.TotalAmount),

                    AttributedRevenue = g.Sum(o => o.OrderItems.Any()
                        ? o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity)
                        : o.TotalAmount),

                    OrderCount = g.Count(),
                    ChannelType = g.Key == "Website" ? "Direct Store" : "Marketplace Referral"
                })
                .OrderByDescending(c => c.AttributedRevenue)
                .ToList();

            var standardChannels = new List<string> { "Website", "Amazon", "Flipkart", "Social Media" };
            var resultList = new List<ChannelSalesSummary>();

            foreach (var ch in standardChannels)
            {
                var existing = channelData.FirstOrDefault(c => c.Channel.Equals(ch, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    resultList.Add(existing);
                }
                else
                {
                    resultList.Add(new ChannelSalesSummary
                    {
                        Channel = ch,
                        TotalRevenue = 0,
                        AttributedRevenue = 0,
                        OrderCount = 0,
                        ChannelType = ch == "Website" ? "Direct Store" : "Marketplace Referral"
                    });
                }
            }

            foreach (var extra in channelData)
            {
                if (!standardChannels.Any(sc => sc.Equals(extra.Channel, StringComparison.OrdinalIgnoreCase)))
                {
                    resultList.Add(extra);
                }
            }

            return resultList;
        }

        public async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            var channelSales = await GetSalesByChannelAsync();
            var confirmedRevenue = await _context.Orders
                .Where(o => o.Status == "Completed")
                .SumAsync(o => o.TotalAmount);

            var totalOrders = await _context.Orders.CountAsync(o => o.Status == "Completed");
            var totalLeads = await _context.Orders.CountAsync(o => o.Status != "Completed");
            var subscriberCount = await _context.Subscribers.CountAsync();
            var activeCampaigns = await _context.Campaigns.CountAsync(c => c.Status == "Active");

            var referredMarketplaceGMV = channelSales
                .Where(c => !c.Channel.Equals("Website", StringComparison.OrdinalIgnoreCase))
                .Sum(c => c.AttributedRevenue);

            var totalGMV = confirmedRevenue + referredMarketplaceGMV;

            return new DashboardSummary
            {
                TotalRevenue = confirmedRevenue,
                ReferredMarketplaceGMV = referredMarketplaceGMV,
                TotalAttributedGMV = totalGMV,
                TotalOrders = totalOrders,
                TotalLeads = totalLeads,
                TotalSubscribers = subscriberCount,
                ActiveCampaigns = activeCampaigns,
                AverageOrderValue = totalOrders > 0 ? Math.Round(confirmedRevenue / totalOrders, 2) : 0,
                SalesByChannel = channelSales
            };
        }
    }
}
