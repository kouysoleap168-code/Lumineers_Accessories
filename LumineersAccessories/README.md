# Lumineers Accessories — ASP.NET Core MVC E-Commerce

An elegant, feminine, modern online accessories store built with **ASP.NET Core MVC**, **C#**, **Entity Framework Core**, **SQL Server**, and **Bootstrap 5**. Designed to open and run in **Microsoft Visual Studio**.

> _"Shine in Your Own Way."_

---

## Features

### Customer
- Register / Login (BCrypt password hashing)
- Browse, search, filter, and sort products
- View product details with related products
- Add to cart (session-based)
- Add to / remove from wishlist
- Checkout with shipping info
- Order confirmation with tracking number
- View order history and order details
- Customer profile dashboard

### Admin
- Dashboard with statistics and a simple sales chart
- Product management (create, edit, delete, details)
- Category management (create, edit, delete — with referential integrity guard)
- Order management (view all, view details, update status)
- Inventory management (update stock levels, low-stock alerts)
- Customer management (view list and order history)
- Shipping management (update tracking numbers)

---

## Database

8 tables in **Third Normal Form (3NF)**:

| Table | Purpose |
|---|---|
| Customers | Registered customers |
| Categories | Product categories (Bracelets, Necklaces, Rings, Earrings) |
| Products | Sellable products |
| Orders | Customer orders |
| OrderItems | Line items (captures PricePerUnit at purchase time) |
| Inventory | One-to-one stock record per product |
| ShippingDetails | One-to-one shipping record per order |
| Wishlists | Saved products (unique per customer + product) |

**Relationships:**
- Category 1 → * Products
- Customer 1 → * Orders
- Order 1 → * OrderItems
- Product 1 → * OrderItems
- Product 1 → 1 Inventory
- Order 1 → 1 ShippingDetail
- Customer 1 → * Wishlists
- Product 1 → * Wishlists

---

## How to Run in Visual Studio

### Prerequisites
- **Visual Studio 2022** (17.0+) with the **.NET 8 SDK** and the **ASP.NET and web development** workload
- **SQL Server** (LocalDB is fine — it's installed with Visual Studio by default)

### Steps

1. **Open the solution** — double-click `LumineersAccessories.csproj` (or use File → Open → Project/Solution).

2. **Restore packages** — Visual Studio restores NuGet packages automatically. You can also run:
   ```
   dotnet restore
   ```

3. **Check the connection string** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LumineersAccessoriesDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
   Change this if your SQL Server instance is different.

4. **Create the database with EF Core migrations** — open the Package Manager Console (Tools → NuGet Package Manager → Package Manager Console) and run:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
   Or from a terminal:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Run the project** — press **F5** (or Ctrl+F5) in Visual Studio, or run:
   ```
   dotnet run
   ```

6. The app opens in your browser at `https://localhost:5001` (or a Visual Studio-assigned port).

### Seed Data
The database is automatically seeded on first run with:
- 4 categories (Bracelets, Necklaces, Rings, Earrings)
- 8 sample products with real photos
- 8 inventory records
- 1 demo admin account

### Demo Admin Account
```
Email:    admin@lumineersaccessories.com
Password: admin123
```
Log in with this account to access the admin dashboard at `/Admin`.

---

## Project Structure

```
LumineersAccessories/
├── Controllers/          Customer-facing controllers
├── Areas/Admin/
│   ├── Controllers/      Admin area controllers
│   └── Views/            Admin area views
├── Data/                 ApplicationDbContext (EF Core)
├── Infrastructure/       Cart service, session helpers, admin attribute
├── Models/               8 entity models
├── ViewModels/           View-specific models
├── Views/                Razor views (shared layout + per-controller)
│   └── Shared/
├── wwwroot/
│   ├── css/site.css      Elegant feminine theme
│   ├── js/site.js       Toast, animations, quantity clamping
│   └── images/
├── appsettings.json
├── Program.cs
└── LumineersAccessories.csproj
```

---

## Tech Stack
- ASP.NET Core MVC 8
- C# 12
- Entity Framework Core 8 (Code First, SQL Server)
- Bootstrap 5.3
- Bootstrap Icons
- BCrypt.Net-Next (password hashing)
- jQuery Validation (client-side validation)

---

## Security Notes
- Passwords are hashed with BCrypt (never stored as plain text)
- Anti-forgery tokens on all POST forms
- Admin pages are protected by an authorization attribute
- EF Core uses parameterized queries (no raw SQL injection risk)
- Connection string lives in `appsettings.json`, not in frontend code

---

© 2026 Lumineers Accessories. All Rights Reserved.
