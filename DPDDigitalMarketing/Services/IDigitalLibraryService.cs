namespace DPDDigitalMarketing.Services
{
    public class IssueSection
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Duration { get; set; } = "5 min read";
        public string Icon { get; set; } = "bi-journal-text";
        public string HtmlContent { get; set; } = string.Empty;
    }

    public class DigitalIssue
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Language { get; set; } = "English";
        public string IssueNumber { get; set; } = "Vol. 51 • Issue 34";
        public string CoverImageUrl { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
        public List<IssueSection> Sections { get; set; } = new();
    }

    public interface IDigitalLibraryService
    {
        DigitalIssue? GetDigitalIssue(int productId);
        List<DigitalIssue> GetAllDigitalIssues();
    }
}
