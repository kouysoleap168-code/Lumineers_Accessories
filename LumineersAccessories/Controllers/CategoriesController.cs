using LumineersAccessories.Data;
using LumineersAccessories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoriesController(ApplicationDbContext db) => _db = db;

        // GET: /Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories
                .Include(c => c.Products)
                .ToListAsync();
            return View(categories);
        }

        // GET: /Categories/Details/5  -> show products in a category
        public async Task<IActionResult> Details(int id)
        {
            var category = await _db.Categories
                .Include(c => c.Products)
                    .ThenInclude(p => p.Inventory)
                .FirstOrDefaultAsync(c => c.CategoryID == id);
            if (category == null) return NotFound();
            return View(category);
        }
    }
}
