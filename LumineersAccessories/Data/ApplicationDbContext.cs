using LumineersAccessories.Models;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Data
{
    /// <summary>
    /// EF Core DbContext for the Lumineers Accessories database.
    /// Relationships, unique constraints, and seed data are configured in OnModelCreating.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Inventory> Inventories { get; set; } = null!;
        public DbSet<ShippingDetail> ShippingDetails { get; set; } = null!;
        public DbSet<Wishlist> Wishlists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Customers: unique email ---
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // --- Category -> Products (one-to-many) ---
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Customer -> Orders (one-to-many) ---
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Order -> OrderItems (one-to-many) ---
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Product -> OrderItems (one-to-many) ---
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Product -> Inventory (one-to-one) ---
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(i => i.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Order -> ShippingDetail (one-to-one) ---
            modelBuilder.Entity<ShippingDetail>()
                .HasOne(s => s.Order)
                .WithOne(o => o.ShippingDetail)
                .HasForeignKey<ShippingDetail>(s => s.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Customer -> Wishlists (one-to-many) ---
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Customer)
                .WithMany(c => c.Wishlists)
                .HasForeignKey(w => w.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Product -> Wishlists (one-to-many) ---
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Product)
                .WithMany(p => p.Wishlists)
                .HasForeignKey(w => w.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Wishlist: a customer can wishlist a product only once ---
            modelBuilder.Entity<Wishlist>()
                .HasIndex(w => new { w.CustomerID, w.ProductID })
                .IsUnique();

            // --- Seed data ---
            SeedCategories(modelBuilder);
            SeedProducts(modelBuilder);
            SeedInventories(modelBuilder);
            SeedAdminCustomer(modelBuilder);
        }

        private static void SeedCategories(ModelBuilder mb)
        {
            mb.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Bracelets" },
                new Category { CategoryID = 2, CategoryName = "Necklaces" },
                new Category { CategoryID = 3, CategoryName = "Rings" },
                new Category { CategoryID = 4, CategoryName = "Earrings" }
            );
        }

        private static void SeedProducts(ModelBuilder mb)
        {
            mb.Entity<Product>().HasData(
                new Product { ProductID = 1, ProductName = "Pearl Glow Bracelet", Price = 12.99m, CategoryID = 1, Description = "A delicate bracelet with softly glowing pearls for everyday elegance.", ImageUrl = "https://images.pexels.com/photos/3641056/pexels-photo-3641056.jpeg?auto=compress&cs=tinysrgb&h=650&w=940", Rating = 5 },
                new Product { ProductID = 2, ProductName = "Golden Heart Necklace", Price = 18.99m, CategoryID = 2, Description = "A golden heart pendant on a fine chain — a little shine close to your heart.", ImageUrl = "https://images.pexels.com/photos/17368716/pexels-photo-17368716.jpeg?auto=compress&cs=tinysrgb&h=650&w=940", Rating = 5 },
                new Product { ProductID = 3, ProductName = "Silver Moon Ring", Price = 10.99m, CategoryID = 3, Description = "A slim silver ring inspired by the curve of a crescent moon.", ImageUrl = "https://images.pexels.com/photos/2899839/pexels-photo-2899839.jpeg?auto=compress&cs=tinysrgb&h=650&w=940", Rating = 4 },
                new Product { ProductID = 4, ProductName = "Crystal Bloom Earrings", Price = 15.99m, CategoryID = 4, Description = "Crystal earrings that bloom like a tiny flower beside your face.", ImageUrl = "https://images.pexels.com/photos/3214241/pexels-photo-3214241.jpeg?auto=compress&cs=tinysrgb&h=650&w=940", Rating = 5 },
                new Product { ProductID = 5, ProductName = "Rose Charm Bracelet", Price = 14.99m, CategoryID = 1, Description = "A soft rose-gold charm bracelet for a gentle, romantic touch.", ImageUrl = "https://images.pexels.com/photos/19288684/pexels-photo-19288684.jpeg?auto=compress&cs=tinysrgb&h=650&w=940", Rating = 4 },
                new Product { ProductID = 6, ProductName = "Celestial Star Necklace", Price = 21.99m, CategoryID = 2, Description = "A celestial star pendant for dreamers who carry the night sky.", ImageUrl = "https://images.pexels.com/photos/17368721/pexels-photo-17368721.jpeg?auto=compress&cs=tinysrgb&h=650&w=940", Rating = 5 },
                new Product { ProductID = 7, ProductName = "Mini Pearl Earrings", Price = 11.99m, CategoryID = 4, Description = "Tiny pearl studs — small details, beautiful impact.", ImageUrl = "https://images.pexels.com/photos/34444202/pexels-photo-34444202.jpeg?auto=compress&cs=tinysrgb&h=650&w=940", Rating = 4 },
                new Product { ProductID = 8, ProductName = "Elegant Silver Ring", Price = 13.99m, CategoryID = 3, Description = "A timeless elegant silver ring for every moment.", ImageUrl = "https://images.pexels.com/photos/10050219/pexels-photo-10050219.jpeg?auto=compress&cs=tinysrgb&h=650&w=940", Rating = 5 }
            );
        }

        private static void SeedInventories(ModelBuilder mb)
        {
            mb.Entity<Inventory>().HasData(
                new Inventory { InventoryID = 1, ProductID = 1, StockLevel = 25, LastUpdated = new DateTime(2026, 1, 1) },
                new Inventory { InventoryID = 2, ProductID = 2, StockLevel = 18, LastUpdated = new DateTime(2026, 1, 1) },
                new Inventory { InventoryID = 3, ProductID = 3, StockLevel = 4, LastUpdated = new DateTime(2026, 1, 1) },
                new Inventory { InventoryID = 4, ProductID = 4, StockLevel = 30, LastUpdated = new DateTime(2026, 1, 1) },
                new Inventory { InventoryID = 5, ProductID = 5, StockLevel = 12, LastUpdated = new DateTime(2026, 1, 1) },
                new Inventory { InventoryID = 6, ProductID = 6, StockLevel = 0, LastUpdated = new DateTime(2026, 1, 1) },
                new Inventory { InventoryID = 7, ProductID = 7, StockLevel = 22, LastUpdated = new DateTime(2026, 1, 1) },
                new Inventory { InventoryID = 8, ProductID = 8, StockLevel = 5, LastUpdated = new DateTime(2026, 1, 1) }
            );
        }

        private static void SeedAdminCustomer(ModelBuilder mb)
        {
            // A demo admin account. Password is "admin123" hashed with BCrypt.
            // In production you would create this through a secure setup flow.
            mb.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerID = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@lumineersaccessories.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Phone = "+855 12 345 678"
                }
            );
        }
    }
}
