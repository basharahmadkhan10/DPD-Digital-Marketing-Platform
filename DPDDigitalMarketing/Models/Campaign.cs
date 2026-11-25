using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPDDigitalMarketing.Models
{

    public class Campaign
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string CampaignName { get; set; } = string.Empty;

        [Required]
        public string Channel { get; set; } = "EmailMarketing";

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; } = 0.00m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal RevenueGenerated { get; set; } = 0.00m;

        public int Impressions { get; set; } = 0;
        public int Clicks { get; set; } = 0;
        public int Conversions { get; set; } = 0;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }

        public string Status { get; set; } = "Active";

        [NotMapped]
        public decimal ROI => Cost > 0 ? ((RevenueGenerated - Cost) / Cost) * 100 : 0;
    }
}
