using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminOnly]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrdersController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _db.SaveChangesAsync();
                TempData["Toast"] = "Order status updated.";
            }
            return RedirectToAction("Details", new { id });
        }
    }
}
