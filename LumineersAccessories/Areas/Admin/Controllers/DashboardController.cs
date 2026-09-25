using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminOnly]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DashboardController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var totalCustomers = await _db.Customers.CountAsync();
            var totalProducts = await _db.Products.CountAsync();
            var totalOrders = await _db.Orders.CountAsync();
            var totalRevenue = await _db.Orders.Where(o => o.Status != "Cancelled").SumAsync(o => o.TotalAmount);

            var lowStock = await _db.Inventories
                .Include(i => i.Product)
                .Where(i => i.StockLevel <= 5)
                .Select(i => new LowStockProductViewModel
                {
                    ProductID = i.ProductID,
                    ProductName = i.Product!.ProductName,
                    StockLevel = i.StockLevel,
                    StockStatus = i.StockLevel == 0 ? "Out of Stock" : "Low Stock"
                })
                .ToListAsync();

            // Simple sales chart: revenue per day for the last 7 days that have orders
            var sales = await _db.Orders
                .GroupBy(o => o.OrderDate.Date)
                .OrderByDescending(g => g.Key)
                .Take(7)
                .Select(g => new SalesPointViewModel { Label = g.Key.ToString("MMM dd"), Total = g.Sum(o => o.TotalAmount) })
                .ToListAsync();
            sales.Reverse();

            var model = new DashboardStatsViewModel
            {
                TotalCustomers = totalCustomers,
                TotalProducts = totalProducts,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                LowStockCount = lowStock.Count,
                LowStockProducts = lowStock,
                SalesByDay = sales
            };
            return View(model);
        }
    }
}
