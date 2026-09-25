using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.Models;
using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _db;
        public WishlistController(ApplicationDbContext db) => _db = db;

        private int? CurrentCustomerID => HttpContext.Session.GetInt32("CustomerID");

        // GET: /Wishlist
        public async Task<IActionResult> Index()
        {
            if (CurrentCustomerID == null)
            {
                TempData["Toast"] = "Please log in to view your wishlist.";
                return RedirectToAction("Login", "Account");
            }
            var id = CurrentCustomerID.Value;
            var items = await _db.Wishlists
                .Include(w => w.Product)
                    .ThenInclude(p => p!.Inventory)
                .Where(w => w.CustomerID == id)
                .ToListAsync();
            return View(items);
        }

        // POST: /Wishlist/Add/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id)
        {
            var cid = CurrentCustomerID;
            if (cid == null)
            {
                TempData["Toast"] = "Please log in to use your wishlist.";
                return RedirectToAction("Login", "Account");
            }

            // Prevent duplicates
            var exists = await _db.Wishlists.AnyAsync(w => w.CustomerID == cid && w.ProductID == id);
            if (!exists)
            {
                _db.Wishlists.Add(new Wishlist { CustomerID = cid.Value, ProductID = id, DateAdded = DateTime.UtcNow });
                await _db.SaveChangesAsync();
                TempData["Toast"] = "Added to your wishlist.";
            }
            else
            {
                TempData["Toast"] = "This product is already in your wishlist.";
            }
            return RedirectToAction("Index");
        }

        // POST: /Wishlist/Remove/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var cid = CurrentCustomerID;
            if (cid == null) return RedirectToAction("Login", "Account");

            var item = await _db.Wishlists.FirstOrDefaultAsync(w => w.CustomerID == cid && w.ProductID == id);
            if (item != null)
            {
                _db.Wishlists.Remove(item);
                await _db.SaveChangesAsync();
                TempData["Toast"] = "Product removed from wishlist.";
            }
            return RedirectToAction("Index");
        }

        // POST: /Wishlist/MoveToCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToCart(int id)
        {
            var cid = CurrentCustomerID;
            if (cid == null) return RedirectToAction("Login", "Account");

            var item = await _db.Wishlists.FirstOrDefaultAsync(w => w.CustomerID == cid && w.ProductID == id);
            if (item != null)
            {
                var product = await _db.Products.Include(p => p.Inventory).FirstOrDefaultAsync(p => p.ProductID == id);
                if (product != null)
                {
                    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                    var stock = product.Inventory?.StockLevel ?? 0;
                    var ci = cart.FirstOrDefault(c => c.ProductID == id);
                    if (ci != null) ci.Quantity = Math.Min(ci.Quantity + 1, stock);
                    else cart.Add(new CartItem { ProductID = id, ProductName = product.ProductName, Price = product.Price, Quantity = 1, ImageUrl = product.ImageUrl, StockLevel = stock });
                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                }
                _db.Wishlists.Remove(item);
                await _db.SaveChangesAsync();
                TempData["Toast"] = "Product moved to cart.";
            }
            return RedirectToAction("Index");
        }
    }
}
