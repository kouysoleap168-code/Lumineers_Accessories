using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db) => _db = db;

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // POST: /Cart/Update  (quantity update from the cart page)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int productID, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.ProductID == productID);
            if (item != null)
            {
                if (quantity <= 0) cart.Remove(item);
                else item.Quantity = Math.Min(quantity, item.StockLevel);
            }
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        // POST: /Cart/Remove/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            cart.RemoveAll(c => c.ProductID == id);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            TempData["Toast"] = "Product removed from cart.";
            return RedirectToAction("Index");
        }

        // GET: /Cart/Count  -> returns the current item count for the navbar badge
        public IActionResult Count()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return Json(new { count = cart.Sum(c => c.Quantity) });
        }
    }
}
