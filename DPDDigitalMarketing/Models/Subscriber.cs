using System.ComponentModel.DataAnnotations;

namespace DPDDigitalMarketing.Models
{

    public class Subscriber
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string Preference { get; set; } = "All";

        public bool IsActive { get; set; } = true;

        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    }
}
