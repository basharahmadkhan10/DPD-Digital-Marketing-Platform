using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPDDigitalMarketing.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = "Journal";

        public string Language { get; set; } = "English";

        public string CoverImageUrl { get; set; } = "/images/default-cover.jpg";

        public int StockQuantity { get; set; } = 100;

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        public string? AmazonUrl { get; set; }
        public string? FlipkartUrl { get; set; }

        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
