# DPD Digital Marketing Platform 🚀
### ASP.NET Core 10 MVC + EF Core + SQL Server + REST Web API

A complete digital marketing and multi-channel online sales solution for **Publications Division (DPD)** journals (*Yojana, Kurukshetra, Aajkal*) and **Employment News / Rozgar Samachar**.

---

## 📁 Project Architecture & Folder Guide

```
DPDDigitalMarketing/
│
├── Models/                     # Data Models & Entities (Unit II & V)
│   ├── Product.cs              # Publications, journals & Employment News issues
│   ├── Order.cs & OrderItem.cs # Sales orders with channel tracking (Website, Amazon, Social)
│   ├── Subscriber.cs           # Email newsletter & job alert subscribers
│   ├── Campaign.cs             # Digital marketing campaigns (SEO, Email, Social, ROI)
│   └── ApplicationUser.cs      # User authentication (Unit IV)
│
├── Data/                       # Database Context & Seed Data (Unit V)
│   └── AppDbContext.cs         # EF Core DbContext with pre-seeded publications & campaigns
│
├── Services/                   # Business Logic & Dependency Injection (Unit IV)
│   ├── IAnalyticsService.cs    # Channel sales aggregation & dashboard summary
│   ├── AnalyticsService.cs     # Implementation
│   ├── IEmailMarketingService.cs # Email campaign dispatcher & subscriber management
│   └── EmailMarketingService.cs  # Implementation
│
├── Controllers/                # MVC Controllers (Unit III)
│   ├── HomeController.cs       # Landing page with featured issues & alert subscriptions
│   ├── ProductController.cs    # CRUD, catalog filtering by language/category, search
│   ├── OrderController.cs      # Direct checkout & outbound referral tracking (Amazon/Flipkart)
│   ├── MarketingController.cs  # Analytics dashboard, campaign management, email blast
│   ├── NewsletterController.cs # Quick email subscriptions
│   │
│   └── Api/                    # REST Web API Controllers (Unit VI - tested with Postman)
│       ├── ProductsApiController.cs   # GET, POST, PUT, DELETE /api/products
│       └── AnalyticsApiController.cs  # GET /api/analytics/channels & /summary
│
├── Views/                      # Razor Views (Clean template ready for your custom styling)
│   ├── Home/                   # Landing page
│   ├── Product/                # Catalog, Product Details, Add Issue, Edit Issue
│   ├── Order/                  # Checkout, Confirmation, Sales History
│   ├── Marketing/              # Dashboard (Chart.js charts), Campaigns table
│   └── Shared/_Layout.cshtml   # Master layout with navbar & Bootstrap 5
│
└── Program.cs                  # Pipeline, DI registrations, Auto-DB creation on start
```

---

## 🏃 How to Run the Project

Open terminal in the project folder:
```bash
cd "c:\Users\99348\OneDrive\Documents\C-sharp\DPDDigitalMarketing"
dotnet run
```

Then open your browser at the displayed local URL (e.g. `http://localhost:5000` or `https://localhost:5001`).

*Note:* When you run the application for the first time, `Program.cs` automatically creates the database and populates it with sample journals (Yojana, Kurukshetra, Aajkal), Employment News weekly editions, and marketing campaigns!

---

## 📮 REST API Endpoints (Unit VI - Test in Postman)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/products` | Get all publications (supports `?category=Journal&search=Yojana`) |
| `GET` | `/api/products/{id}` | Get publication details by ID |
| `POST` | `/api/products` | Create a new publication (JSON body) |
| `PUT` | `/api/products/{id}` | Update an existing publication |
| `DELETE` | `/api/products/{id}` | Soft-deactivate a publication |
| `GET` | `/api/analytics/channels` | Returns channel sales data for Chart.js |
| `GET` | `/api/analytics/campaign-roi` | Returns ROI & conversions for marketing campaigns |
| `GET` | `/api/analytics/summary` | Returns total revenue, orders, subscriber count |

---

## 🎨 How to Customize the Frontend Design

All views are inside the `Views/` directory:
1. **Master Header/Footer/Navbar**: Edit [Views/Shared/_Layout.cshtml](file:///c:/Users/99348/OneDrive/Documents/C-sharp/DPDDigitalMarketing/Views/Shared/_Layout.cshtml)
2. **Landing Page**: Edit [Views/Home/Index.cshtml](file:///c:/Users/99348/OneDrive/Documents/C-sharp/DPDDigitalMarketing/Views/Home/Index.cshtml)
3. **Publications Catalog**: Edit [Views/Product/Index.cshtml](file:///c:/Users/99348/OneDrive/Documents/C-sharp/DPDDigitalMarketing/Views/Product/Index.cshtml)
4. **Single Issue / Product Page**: Edit [Views/Product/Details.cshtml](file:///c:/Users/99348/OneDrive/Documents/C-sharp/DPDDigitalMarketing/Views/Product/Details.cshtml)
5. **Analytics Dashboard**: Edit [Views/Marketing/Dashboard.cshtml](file:///c:/Users/99348/OneDrive/Documents/C-sharp/DPDDigitalMarketing/Views/Marketing/Dashboard.cshtml)
6. **Checkout Page**: Edit [Views/Order/Checkout.cshtml](file:///c:/Users/99348/OneDrive/Documents/C-sharp/DPDDigitalMarketing/Views/Order/Checkout.cshtml)

You can easily replace the Bootstrap classes with Tailwind, your custom CSS in `wwwroot/css/site.css`, or any HTML template you like!
