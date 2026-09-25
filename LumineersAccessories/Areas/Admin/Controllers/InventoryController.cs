using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminOnly]
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public InventoryController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var inventory = await _db.Inventories
                .Include(i => i.Product)
                .OrderBy(i => i.Product!.ProductName)
                .ToListAsync();
            return View(inventory);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inventory = await _db.Inventories.Include(i => i.Product).FirstOrDefaultAsync(i => i.InventoryID == id);
            if (inventory == null) return NotFound();
            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int stockLevel)
        {
            var inventory = await _db.Inventories.FindAsync(id);
            if (inventory == null) return NotFound();

            inventory.StockLevel = Math.Max(0, stockLevel);
            inventory.LastUpdated = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Stock level updated.";
            return RedirectToAction("Index");
        }
    }
}
