namespace DPDDigitalMarketing.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public string CoverImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public decimal Total => Price * Quantity;
    }
}
