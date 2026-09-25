using LumineersAccessories.Data;
using LumineersAccessories.Models;
using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories.ToListAsync();
            var products = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Inventory)
                .Take(8)
                .ToListAsync();

            var model = new HomeViewModel
            {
                Categories = categories,
                FeaturedProducts = products
            };
            return View(model);
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? code = null)
        {
            Response.StatusCode = code ?? 500;
            return View("~/Views/Shared/Error.cshtml", code ?? 500);
        }
    }

    public class HomeViewModel
    {
        public List<Category> Categories { get; set; } = new();
        public List<Product> FeaturedProducts { get; set; } = new();
    }
}
