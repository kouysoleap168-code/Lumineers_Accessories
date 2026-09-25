using LumineersAccessories.Data;
using LumineersAccessories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrdersController(ApplicationDbContext db) => _db = db;

        private int? CurrentCustomerID => HttpContext.Session.GetInt32("CustomerID");

        // GET: /Orders/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.ShippingDetail)
                .FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: /Orders  -> order history for the logged-in customer
        public async Task<IActionResult> Index()
        {
            var cid = CurrentCustomerID;
            if (cid == null)
            {
                TempData["Toast"] = "Please log in to view your orders.";
                return RedirectToAction("Login", "Account");
            }
            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.CustomerID == cid.Value)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.ShippingDetail)
                .FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null) return NotFound();
            return View(order);
        }
    }
}
