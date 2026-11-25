using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Models;
using DPDDigitalMarketing.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=dpd_marketing.db"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IEmailMarketingService, EmailMarketingService>();
builder.Services.AddScoped<IDigitalLibraryService, DigitalLibraryService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        context.Database.EnsureCreated();

        var p4 = context.Products.FirstOrDefault(p => p.Id == 4);
        if (p4 != null)
        {
            p4.CoverImageUrl = "/images/kurukshetra.jpg";
            p4.AmazonUrl = "https://www.amazon.in/s?k=Kurukshetra+journal+Publications+Division";
            p4.FlipkartUrl = "https://www.flipkart.com/search?q=Kurukshetra+journal";
        }
        var p1 = context.Products.FirstOrDefault(p => p.Id == 1);
        if (p1 != null)
        {
            p1.AmazonUrl = "https://www.amazon.in/s?k=Employment+News+weekly+Publications+Division";
            p1.FlipkartUrl = "https://www.flipkart.com/search?q=Employment+News+government";
        }
        var p2 = context.Products.FirstOrDefault(p => p.Id == 2);
        if (p2 != null)
        {
            p2.AmazonUrl = "https://www.amazon.in/s?k=Rozgar+Samachar+Publications+Division";
            p2.FlipkartUrl = "https://www.flipkart.com/search?q=Rozgar+Samachar";
        }
        var p3 = context.Products.FirstOrDefault(p => p.Id == 3);
        if (p3 != null)
        {
            p3.AmazonUrl = "https://www.amazon.in/s?k=Yojana+magazine+Publications+Division";
            p3.FlipkartUrl = "https://www.flipkart.com/search?q=Yojana+magazine";
        }
        var p5 = context.Products.FirstOrDefault(p => p.Id == 5);
        if (p5 != null)
        {
            p5.AmazonUrl = "https://www.amazon.in/s?k=Aajkal+magazine+Publications+Division";
            p5.FlipkartUrl = "https://www.flipkart.com/search?q=Aajkal+magazine";
        }

        if (!context.Products.Any(p => p.Title.Contains("Defence & Armed Forces")))
        {
            context.Products.Add(new Product
            {
                Title = "Employment News - Defence & Armed Forces Special (Nov 2026)",
                Description = "All-India recruitment edition for Armed Forces: CDS II, NDA, Indian Navy SSR/MR, Air Force Agniveervayu, and CAPF Assistant Commandant notifications (12,800+ posts).",
                Price = 20.00m,
                Category = "EmploymentNews",
                Language = "English",
                StockQuantity = 350,
                CoverImageUrl = "https://images.unsplash.com/photo-1541872703-74c5e44368f9?w=500&auto=format&fit=crop&q=60",
                AmazonUrl = "https://www.amazon.in/s?k=Employment+News+defence+special",
                FlipkartUrl = "https://www.flipkart.com/search?q=Employment+News+defence",
                MetaKeywords = "Defence jobs, NDA vacancy, CDS recruitment, military jobs",
                MetaDescription = "Official Defence Special edition of Employment News.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!context.Products.Any(p => p.Title.Contains("Engineering & PSU")))
        {
            context.Products.Add(new Product
            {
                Title = "Employment News - Engineering & PSU Special (Nov 2026)",
                Description = "Premier recruitment notice for technical graduates: Public Sector Undertakings via GATE (IOCL, NTPC, ONGC, BHEL), State DISCOMs, and ISRO Scientist/Engineer vacancies (8,400+ technical jobs).",
                Price = 20.00m,
                Category = "EmploymentNews",
                Language = "English",
                StockQuantity = 400,
                CoverImageUrl = "https://images.unsplash.com/photo-1581092160607-ee22621dd758?w=500&auto=format&fit=crop&q=60",
                AmazonUrl = "https://www.amazon.in/s?k=Employment+News+engineering+psu",
                FlipkartUrl = "https://www.flipkart.com/search?q=Employment+News+engineering",
                MetaKeywords = "PSU recruitment, GATE engineering jobs, ISRO scientist vacancy",
                MetaDescription = "Official PSU & Engineering recruitment notices in Employment News.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!context.Products.Any(p => p.Title.Contains("Banking & Financial")))
        {
            context.Products.Add(new Product
            {
                Title = "Employment News - Banking & Financial Sector (Nov 2026)",
                Description = "All-India public sector bank vacancies: IBPS PO XIV, SBI Specialist Officers, RBI Grade B & Assistant, and LIC AAO recruitment breakdown (14,200+ financial vacancies).",
                Price = 20.00m,
                Category = "EmploymentNews",
                Language = "English",
                StockQuantity = 500,
                CoverImageUrl = "https://images.unsplash.com/photo-1559526324-4b87b5e36e44?w=500&auto=format&fit=crop&q=60",
                AmazonUrl = "https://www.amazon.in/s?k=Employment+News+banking+jobs",
                FlipkartUrl = "https://www.flipkart.com/search?q=Employment+News+bank+recruitment",
                MetaKeywords = "Bank PO jobs, IBPS recruitment, SBI Specialist Officer",
                MetaDescription = "Banking and financial sector recruitment notices by Publications Division.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!context.Products.Any(p => p.Title.Contains("Rozgar Samachar (Urdu Weekly)")))
        {
            context.Products.Add(new Product
            {
                Title = "Rozgar Samachar (Urdu Weekly) - Rozgar Aur Hunar",
                Description = "Weekly Urdu employment notice carrying central, state, and autonomous university vacancies, Skill India initiatives, and government competitive exam calendars.",
                Price = 20.00m,
                Category = "EmploymentNews",
                Language = "Urdu",
                StockQuantity = 220,
                CoverImageUrl = "https://images.unsplash.com/photo-1457369804613-52c61a468e7d?w=500&auto=format&fit=crop&q=60",
                AmazonUrl = "https://www.amazon.in/s?k=Rozgar+Samachar+Urdu",
                FlipkartUrl = "https://www.flipkart.com/search?q=Rozgar+Samachar+Urdu",
                MetaKeywords = "Rozgar samachar urdu, sarkari naukri urdu, employment news urdu",
                MetaDescription = "Official Urdu edition of Rozgar Samachar by Publications Division.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        context.SaveChanges();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
        }
        if (!roleManager.RoleExistsAsync("Customer").GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();
        }

        var adminEmail = "admin@dpd.gov.in";
        var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "DPD Chief Editor (Admin)",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            var createRes = userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();
            if (createRes.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating/migrating the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
