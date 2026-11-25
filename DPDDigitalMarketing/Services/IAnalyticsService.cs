namespace DPDDigitalMarketing.Services
{
    public class ChannelSalesSummary
    {
        public string Channel { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public decimal AttributedRevenue { get; set; }
        public int OrderCount { get; set; }
        public string ChannelType { get; set; } = "Direct Store";
    }

    public class DashboardSummary
    {
        public decimal TotalRevenue { get; set; }
        public decimal ReferredMarketplaceGMV { get; set; }
        public decimal TotalAttributedGMV { get; set; }
        public int TotalOrders { get; set; }
        public int TotalLeads { get; set; }
        public int TotalSubscribers { get; set; }
        public int ActiveCampaigns { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<ChannelSalesSummary> SalesByChannel { get; set; } = new();
    }

    public interface IAnalyticsService
    {
        Task<DashboardSummary> GetDashboardSummaryAsync();
        Task<List<ChannelSalesSummary>> GetSalesByChannelAsync();
    }
}
