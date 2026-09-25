using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.Models;
using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db) => _db = db;

        // GET: /Products?search=&categoryID=&minPrice=&maxPrice=&sort=
        public async Task<IActionResult> Index(string? search, int? categoryID, decimal? minPrice, decimal? maxPrice, string sort = "newest")
        {
            var query = _db.Products.Include(p => p.Category).Include(p => p.Inventory).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(s) || (p.Category != null && p.Category.CategoryName.ToLower().Contains(s)));
            }
            if (categoryID.HasValue) query = query.Where(p => p.CategoryID == categoryID.Value);
            if (minPrice.HasValue) query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(p => p.Price <= maxPrice.Value);

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "popular" => query.OrderByDescending(p => p.Rating),
                _ => query.OrderByDescending(p => p.ProductID)
            };

            var model = new ProductFilterViewModel
            {
                Search = search,
                CategoryID = categoryID,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Sort = sort,
                Categories = await _db.Categories.ToListAsync(),
                Products = await query.ToListAsync()
            };
            return View(model);
        }

        // GET: /Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null) return NotFound();

            var related = await _db.Products
                .Include(p => p.Inventory)
                .Where(p => p.CategoryID == product.CategoryID && p.ProductID != product.ProductID)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedProducts = related;
            return View(product);
        }

        // POST: /Products/AddToCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var product = await _db.Products.Include(p => p.Inventory).FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null) return NotFound();
            var stock = product.Inventory?.StockLevel ?? 0;
            if (quantity < 1) quantity = 1;
            if (quantity > stock) quantity = stock;

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.ProductID == id);
            if (item != null)
                item.Quantity = Math.Min(item.Quantity + quantity, stock);
            else
                cart.Add(new CartItem
                {
                    ProductID = id,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl,
                    StockLevel = stock
                });
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            TempData["Toast"] = "Product added to cart.";
            return RedirectToAction("Index", "Cart");
        }
    }
}
