using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminOnly]
    public class ShippingController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ShippingController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var shipping = await _db.ShippingDetails
                .Include(s => s.Order)
                    .ThenInclude(o => o!.Customer)
                .OrderByDescending(s => s.ShippingID)
                .ToListAsync();
            return View(shipping);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var shipping = await _db.ShippingDetails
                .Include(s => s.Order)
                    .ThenInclude(o => o!.Customer)
                .FirstOrDefaultAsync(s => s.ShippingID == id);
            if (shipping == null) return NotFound();
            return View(shipping);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string trackingNumber)
        {
            var shipping = await _db.ShippingDetails.FindAsync(id);
            if (shipping == null) return NotFound();
            shipping.TrackingNumber = trackingNumber;
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Tracking number updated.";
            return RedirectToAction("Index");
        }
    }
}
