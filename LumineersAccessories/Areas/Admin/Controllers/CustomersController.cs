using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminOnly]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CustomersController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var customers = await _db.Customers
                .Select(c => new CustomerWithOrderCountViewModel
                {
                    CustomerID = c.CustomerID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    OrderCount = _db.Orders.Count(o => o.CustomerID == c.CustomerID)
                })
                .ToListAsync();
            return View(customers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.CustomerID == id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            ViewBag.Orders = orders;
            return View(customer);
        }
    }
}
