using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Models;

namespace DPDDigitalMarketing.Services
{
    public class EmailMarketingService : IEmailMarketingService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmailMarketingService> _logger;
        private readonly IConfiguration _config;

        public EmailMarketingService(AppDbContext context, ILogger<EmailMarketingService> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }

        private async Task SendMailtrapEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                var host = _config["SmtpSettings:Host"] ?? "sandbox.smtp.mailtrap.io";
                var portStr = _config["SmtpSettings:Port"] ?? "2525";
                int port = int.TryParse(portStr, out int p) ? p : 2525;
                var user = _config["SmtpSettings:Username"] ?? "6c7944fa463243";
                var pass = _config["SmtpSettings:Password"] ?? "34cb0799e4fe21";
                var fromEmail = _config["SmtpSettings:SenderEmail"] ?? "alerts@dpd.gov.in";
                var fromName = _config["SmtpSettings:SenderName"] ?? "Publications Division (DPD)";

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(user, pass),
                    EnableSsl = true
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);
                await client.SendMailAsync(message);
                _logger.LogInformation("Email successfully dispatched via Mailtrap to {Recipient}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email via Mailtrap to {Recipient}", toEmail);
            }
        }

        public async Task<bool> SubscribeAsync(string email, string fullName, string preference)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var existing = await _context.Subscribers.FirstOrDefaultAsync(s => s.Email == email);
            if (existing != null)
            {
                existing.IsActive = true;
                existing.Preference = preference;
            }
            else
            {
                _context.Subscribers.Add(new Subscriber
                {
                    Email = email.Trim().ToLower(),
                    FullName = fullName?.Trim(),
                    Preference = preference,
                    SubscribedAt = DateTime.UtcNow,
                    IsActive = true
                });
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("New subscriber added: {Email} for {Preference}", email, preference);

            var baseUrl = _config["AppBaseUrl"] ?? "http://localhost:5050";

            var welcomeHtml = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 12px; background: #ffffff;'>
    <div style='background-color: #0f172a; padding: 18px; border-radius: 8px; text-align: center;'>
        <h2 style='color: #ffffff; margin: 0; font-size: 20px;'>Publications Division (DPD)</h2>
        <p style='color: #f97316; margin: 6px 0 0 0; font-size: 13px; font-weight: bold;'>Ministry of Information & Broadcasting, Govt. of India</p>
    </div>
    <div style='padding: 24px 8px;'>
        <h3 style='color: #1e293b; margin-top: 0;'>Subscription Confirmed!</h3>
        <p style='color: #475569; font-size: 14px; line-height: 1.6;'>Hello <b>{(string.IsNullOrEmpty(fullName) ? "Aspirant" : fullName)}</b>,</p>
        <p style='color: #475569; font-size: 14px; line-height: 1.6;'>You have successfully subscribed to the <b>DPD Digital Publications & Employment News</b> alert service. You will receive direct notifications for newly released UPSC, SSC, Banking, and Defence recruitment notices, alongside flagship editions of Yojana and Kurukshetra.</p>
        <div style='background-color: #f8fafc; border-left: 4px solid #f97316; padding: 12px 16px; margin: 18px 0; border-radius: 4px;'>
            <div style='font-size: 13px; color: #334155;'><b>Alert Category:</b> {preference}</div>
            <div style='font-size: 13px; color: #334155;'><b>Status:</b> Active Verified Subscriber</div>
        </div>
        <div style='margin: 25px 0; text-align: center;'>
            <a href='{baseUrl}/Product' target='_blank' rel='noopener noreferrer' style='background-color: #0f172a; color: #ffffff; padding: 12px 28px; border-radius: 50px; text-decoration: none; font-weight: bold; font-size: 14px; display: inline-block;'>View Latest Gazettes & Issues</a>
        </div>
        <p style='color: #94a3b8; font-size: 12px; margin-top: 25px;'>This is an automated notification from the DPD Digital Marketing Platform.</p>
    </div>
</div>";

            _ = Task.Run(() => SendMailtrapEmailAsync(email, "Welcome to DPD Job Alerts & Publications Gazette", welcomeHtml));

            return true;
        }

        public async Task<int> SendJobAlertBlastAsync(string subject, string messageContent, string targetGroup)
        {
            var query = _context.Subscribers.Where(s => s.IsActive);
            if (targetGroup != "All")
            {
                query = query.Where(s => s.Preference == targetGroup || s.Preference == "All");
            }

            var recipients = await query.ToListAsync();

            var campaign = new Campaign
            {
                CampaignName = subject,
                Channel = "EmailMarketing",
                Cost = recipients.Count * 0.10m,
                RevenueGenerated = recipients.Count * 1.50m,
                Impressions = recipients.Count,
                Clicks = (int)(recipients.Count * 0.35),
                Conversions = (int)(recipients.Count * 0.08),
                StartDate = DateTime.UtcNow,
                Status = "Completed"
            };

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            var baseUrl = _config["AppBaseUrl"] ?? "http://localhost:5050";

            var blastHtml = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 12px; background: #ffffff;'>
    <div style='background-color: #0f172a; padding: 18px; border-radius: 8px; text-align: center;'>
        <h2 style='color: #ffffff; margin: 0; font-size: 20px;'>Official Job & Gazette Alert</h2>
        <p style='color: #f97316; margin: 6px 0 0 0; font-size: 13px; font-weight: bold;'>Publications Division (DPD), Govt. of India</p>
    </div>
    <div style='padding: 24px 8px;'>
        <h3 style='color: #1e293b; margin-top: 0;'>{subject}</h3>
        <p style='color: #475569; font-size: 14px; line-height: 1.6;'>{messageContent}</p>
        <div style='margin: 25px 0; text-align: center;'>
            <a href='{baseUrl}/Product' target='_blank' rel='noopener noreferrer' style='background-color: #0f172a; color: #ffffff; padding: 12px 28px; border-radius: 50px; text-decoration: none; font-weight: bold; font-size: 14px; display: inline-block;'>View Latest Gazettes & Apply</a>
        </div>
        <p style='color: #94a3b8; font-size: 12px; margin-top: 25px;'>You received this notification because you subscribed to DPD Official Alerts.</p>
    </div>
</div>";

            _ = Task.Run(async () =>
            {
                foreach (var r in recipients.Take(10))
                {
                    await SendMailtrapEmailAsync(r.Email, $"[DPD Alert] {subject}", blastHtml);
                }
            });

            return recipients.Count;
        }

        public Task SendOrderConfirmationEmailAsync(Order order, List<CartItem> items)
        {
            if (string.IsNullOrWhiteSpace(order.CustomerEmail))
            {
                return Task.CompletedTask;
            }

            var baseUrl = _config["AppBaseUrl"] ?? "https://dpd-digital-marketing-platform-1.onrender.com";
            var customerName = WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(order.CustomerName) ? "Valued Reader" : order.CustomerName);

            var rowsHtml = string.Join("", items.Select(item => $@"
                <tr>
                    <td style='padding: 10px 8px; border-bottom: 1px solid #e2e8f0; font-size: 13px; color: #1e293b;'>
                        <b>{WebUtility.HtmlEncode(item.Title)}</b><br/>
                        <span style='color: #64748b; font-size: 11px;'>{WebUtility.HtmlEncode(item.Category)} • {WebUtility.HtmlEncode(item.Language)}</span>
                    </td>
                    <td style='padding: 10px 8px; border-bottom: 1px solid #e2e8f0; font-size: 13px; color: #334155; text-align: center;'>{item.Quantity}</td>
                    <td style='padding: 10px 8px; border-bottom: 1px solid #e2e8f0; font-size: 13px; color: #0f172a; font-weight: bold; text-align: right;'>&#8377;{item.Total:0.00}</td>
                </tr>"));

            var orderHtml = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 12px; background: #ffffff;'>
    <div style='background-color: #0f172a; padding: 18px; border-radius: 8px; text-align: center;'>
        <h2 style='color: #ffffff; margin: 0; font-size: 20px;'>Publications Division (DPD)</h2>
        <p style='color: #f97316; margin: 6px 0 0 0; font-size: 13px; font-weight: bold;'>Order Confirmed • Digital Edition Unlocked</p>
    </div>
    <div style='padding: 24px 8px;'>
        <h3 style='color: #16a34a; margin-top: 0;'>&#10004; Thank you for your order, {customerName}!</h3>
        <p style='color: #475569; font-size: 14px; line-height: 1.6;'>Your payment is confirmed and your digital publications are now unlocked in your <b>My Library</b> section.</p>

        <div style='background-color: #f8fafc; border-left: 4px solid #f97316; padding: 12px 16px; margin: 18px 0; border-radius: 4px;'>
            <div style='font-size: 13px; color: #334155;'><b>Order ID:</b> #DPD-{order.Id}</div>
            <div style='font-size: 13px; color: #334155;'><b>Order Date:</b> {order.OrderDate:dd MMM yyyy, hh:mm tt} UTC</div>
            <div style='font-size: 13px; color: #334155;'><b>Channel:</b> {WebUtility.HtmlEncode(order.SaleChannel)}</div>
            <div style='font-size: 13px; color: #334155;'><b>Total Paid:</b> <span style='color: #0f172a; font-weight: bold;'>&#8377;{order.TotalAmount:0.00}</span></div>
        </div>

        <table style='width: 100%; border-collapse: collapse; margin: 16px 0;'>
            <thead>
                <tr style='background-color: #f1f5f9;'>
                    <th style='padding: 10px 8px; text-align: left; font-size: 12px; color: #475569;'>Publication</th>
                    <th style='padding: 10px 8px; text-align: center; font-size: 12px; color: #475569;'>Qty</th>
                    <th style='padding: 10px 8px; text-align: right; font-size: 12px; color: #475569;'>Amount</th>
                </tr>
            </thead>
            <tbody>
                {rowsHtml}
            </tbody>
        </table>

        <div style='margin: 26px 0; text-align: center;'>
            <a href='{baseUrl}/Library' target='_blank' rel='noopener noreferrer' style='background-color: #0f172a; color: #ffffff; padding: 12px 24px; border-radius: 50px; text-decoration: none; font-weight: bold; font-size: 14px; display: inline-block; margin: 4px;'>Read in My Library</a>
            <a href='{baseUrl}/Order/Confirmation/{order.Id}' target='_blank' rel='noopener noreferrer' style='background-color: #f97316; color: #ffffff; padding: 12px 24px; border-radius: 50px; text-decoration: none; font-weight: bold; font-size: 14px; display: inline-block; margin: 4px;'>View Order Receipt</a>
        </div>

        <p style='color: #94a3b8; font-size: 12px; margin-top: 25px;'>This is an official order confirmation email from the DPD Digital Marketing Platform.</p>
    </div>
</div>";

            var recipient = order.CustomerEmail;
            var orderId = order.Id;
            _ = Task.Run(() => SendMailtrapEmailAsync(recipient, $"Order Confirmed #DPD-{orderId} - Publications Division (DPD)", orderHtml));

            return Task.CompletedTask;
        }
    }
}
