using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminOnly]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoriesController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories
                .Include(c => c.Products)
                .ToListAsync();
            return View(categories);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryID || !ModelState.IsValid) return View(category);
            _db.Update(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryID == id);
            if (category == null) return RedirectToAction("Index");

            if (category.Products.Any())
            {
                ModelState.AddModelError("", "Cannot delete a category that still has products. Reassign the products first.");
                TempData["Toast"] = "Cannot delete a category that still has products.";
                return RedirectToAction("Index");
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
