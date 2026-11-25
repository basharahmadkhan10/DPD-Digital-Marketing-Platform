using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Models;

namespace DPDDigitalMarketing.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Employment News (Weekly) - November 2026 Issue",
                    Description = "Official Government of India job publication with latest UPSC, SSC, Banking, Railway, and Defence vacancies.",
                    Price = 20.00m,
                    Category = "EmploymentNews",
                    Language = "English",
                    StockQuantity = 500,
                    CoverImageUrl = "https://images.unsplash.com/photo-1585829365295-ab7cd400c167?w=500&auto=format&fit=crop&q=60",
                    AmazonUrl = "https://www.amazon.in/s?k=Employment+News+weekly+Publications+Division",
                    FlipkartUrl = "https://www.flipkart.com/search?q=Employment+News+government",
                    MetaKeywords = "Employment news, government jobs, sarkari naukri, upsc vacancies, ssc jobs",
                    MetaDescription = "Buy official Employment News weekly issue online with full government job notifications.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 1)
                },
                new Product
                {
                    Id = 2,
                    Title = "Rozgar Samachar (Hindi Weekly) - Nov 2026",
                    Description = "Sarkari naukri patrika - kendriya evam rajya sarkari bhartiya, UPSC, SSC, Police avam Bank vacancy details.",
                    Price = 20.00m,
                    Category = "EmploymentNews",
                    Language = "Hindi",
                    StockQuantity = 450,
                    CoverImageUrl = "https://images.unsplash.com/photo-1504711434969-e33886168f5c?w=500&auto=format&fit=crop&q=60",
                    AmazonUrl = "https://www.amazon.in/s?k=Rozgar+Samachar+Publications+Division",
                    FlipkartUrl = "https://www.flipkart.com/search?q=Rozgar+Samachar",
                    MetaKeywords = "Rozgar samachar, sarkari vacancy, hindi job news",
                    MetaDescription = "Rozgar Samachar Hindi edition - official DPD weekly publication.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 1)
                },
                new Product
                {
                    Id = 3,
                    Title = "Yojana Monthly Journal - Socio-Economic Development",
                    Description = "Flagship socio-economic monthly journal published by Publications Division (DPD). Highly recommended for Civil Services aspirants.",
                    Price = 30.00m,
                    Category = "Journal",
                    Language = "English",
                    StockQuantity = 300,
                    CoverImageUrl = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=500&auto=format&fit=crop&q=60",
                    AmazonUrl = "https://www.amazon.in/s?k=Yojana+magazine+Publications+Division",
                    FlipkartUrl = "https://www.flipkart.com/search?q=Yojana+magazine",
                    MetaKeywords = "Yojana magazine, DPD journals, UPSC current affairs, rural economy",
                    MetaDescription = "Buy latest Yojana journal online by Publications Division, Ministry of I&B.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 5)
                },
                new Product
                {
                    Id = 4,
                    Title = "Kurukshetra - Monthly Rural Development Journal",
                    Description = "A journal on rural development dedicated to agriculture, Panchayati Raj, and rural transformation.",
                    Price = 30.00m,
                    Category = "Journal",
                    Language = "English",
                    StockQuantity = 250,
                    CoverImageUrl = "/images/kurukshetra.jpg",
                    AmazonUrl = "https://www.amazon.in/s?k=Kurukshetra+journal+Publications+Division",
                    FlipkartUrl = "https://www.flipkart.com/search?q=Kurukshetra+journal",
                    MetaKeywords = "Kurukshetra journal, rural development, agriculture policy",
                    MetaDescription = "Official Kurukshetra journal for policy makers, students, and competitive exams.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 5)
                },
                new Product
                {
                    Id = 5,
                    Title = "Aajkal (Urdu) - Literary & Cultural Journal",
                    Description = "Prestigious literary journal carrying poetry, critical essays, short stories, and cultural heritage.",
                    Price = 25.00m,
                    Category = "Journal",
                    Language = "Urdu",
                    StockQuantity = 150,
                    CoverImageUrl = "https://images.unsplash.com/photo-1457369804613-52c61a468e7d?w=500&auto=format&fit=crop&q=60",
                    AmazonUrl = "https://www.amazon.in/s?k=Aajkal+magazine+Publications+Division",
                    FlipkartUrl = "https://www.flipkart.com/search?q=Aajkal+magazine",
                    MetaKeywords = "Aajkal urdu, DPD literature magazine",
                    MetaDescription = "Aajkal Urdu literary publication by Publications Division.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 10)
                }
            );

            builder.Entity<Campaign>().HasData(
                new Campaign
                {
                    Id = 1,
                    CampaignName = "Weekly Job Alert Email Blast",
                    Channel = "EmailMarketing",
                    Cost = 1500.00m,
                    RevenueGenerated = 18400.00m,
                    Impressions = 25000,
                    Clicks = 4200,
                    Conversions = 620,
                    StartDate = new DateTime(2026, 11, 1),
                    Status = "Active"
                },
                new Campaign
                {
                    Id = 2,
                    CampaignName = "Google SEO - 'Sarkari Naukri Employment News'",
                    Channel = "SEO",
                    Cost = 2000.00m,
                    RevenueGenerated = 34500.00m,
                    Impressions = 80000,
                    Clicks = 11500,
                    Conversions = 1150,
                    StartDate = new DateTime(2026, 10, 15),
                    Status = "Active"
                },
                new Campaign
                {
                    Id = 3,
                    CampaignName = "UPSC Aspirants Facebook / Instagram Ads",
                    Channel = "SocialMedia_Facebook",
                    Cost = 5000.00m,
                    RevenueGenerated = 22000.00m,
                    Impressions = 60000,
                    Clicks = 3800,
                    Conversions = 440,
                    StartDate = new DateTime(2026, 11, 3),
                    Status = "Active"
                },
                new Campaign
                {
                    Id = 4,
                    CampaignName = "Amazon Sponsored Products (Yojana & Kurukshetra)",
                    Channel = "Amazon",
                    Cost = 3500.00m,
                    RevenueGenerated = 15800.00m,
                    Impressions = 35000,
                    Clicks = 2900,
                    Conversions = 310,
                    StartDate = new DateTime(2026, 11, 2),
                    Status = "Active"
                }
            );
        }
    }
}
