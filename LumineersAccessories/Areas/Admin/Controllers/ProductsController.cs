using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminOnly]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var products = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Inventory)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            if (!ModelState.IsValid) return View(product);

            _db.Products.Add(product);
            // Create an inventory record for the new product
            _db.Inventories.Add(new Inventory { ProductID = product.ProductID, StockLevel = 0, LastUpdated = DateTime.UtcNow });
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            if (id != product.ProductID || !ModelState.IsValid) return View(product);

            try
            {
                _db.Update(product);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Products.AnyAsync(p => p.ProductID == id)) return NotFound();
                throw;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
