using LumineersAccessories.Data;
using LumineersAccessories.Models;
using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AccountController(ApplicationDbContext db) => _db = db;

        // GET: /Account/Login
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (customer == null || !BCrypt.Net.BCrypt.Verify(model.Password, customer.Password))
            {
                ModelState.AddModelError("", "The email or password is incorrect.");
                return View(model);
            }

            HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
            HttpContext.Session.SetString("CustomerEmail", customer.Email);
            HttpContext.Session.SetString("CustomerName", customer.FullName);

            // Admin goes to the dashboard; everyone else to their profile
            if (customer.Email == "admin@lumineersaccessories.com")
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return RedirectToAction("Profile");
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var exists = await _db.Customers.AnyAsync(c => c.Email == model.Email);
            if (exists)
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
                return View(model);
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            TempData["Toast"] = "Account created. Please log in.";
            return RedirectToAction("Login");
        }

        // GET: /Account/Profile
        public async Task<IActionResult> Profile()
        {
            var cid = HttpContext.Session.GetInt32("CustomerID");
            if (cid == null) return RedirectToAction("Login");

            var customer = await _db.Customers.FindAsync(cid.Value);
            if (customer == null) return RedirectToAction("Login");

            var recentOrders = await _db.Orders
                .Where(o => o.CustomerID == cid.Value)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();
            var wishlistCount = await _db.Wishlists.CountAsync(w => w.CustomerID == cid.Value);

            ViewBag.RecentOrders = recentOrders;
            ViewBag.WishlistCount = wishlistCount;
            return View(customer);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
