using DPDDigitalMarketing.Models;

namespace DPDDigitalMarketing.Services
{
    public interface IEmailMarketingService
    {
        Task<bool> SubscribeAsync(string email, string fullName, string preference);
        Task<int> SendJobAlertBlastAsync(string subject, string messageContent, string targetGroup);
        Task SendOrderConfirmationEmailAsync(Order order, List<CartItem> items);
    }
}
