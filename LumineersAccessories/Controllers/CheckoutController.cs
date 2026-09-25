using LumineersAccessories.Data;
using LumineersAccessories.Infrastructure;
using LumineersAccessories.Models;
using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumineersAccessories.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CheckoutController(ApplicationDbContext db) => _db = db;

        private int? CurrentCustomerID => HttpContext.Session.GetInt32("CustomerID");

        // GET: /Checkout
        public async Task<IActionResult> Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["Toast"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel { CartItems = cart };

            // Pre-fill from the logged-in customer if available
            var cid = CurrentCustomerID;
            if (cid != null)
            {
                var customer = await _db.Customers.FindAsync(cid.Value);
                if (customer != null)
                {
                    model.FirstName = customer.FirstName;
                    model.LastName = customer.LastName;
                    model.Email = customer.Email;
                    model.Phone = customer.Phone;
                }
            }
            return View(model);
        }

        // POST: /Checkout/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            model.CartItems = cart;
            if (!cart.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View("Index", model);
            }
            if (!ModelState.IsValid) return View("Index", model);

            // Resolve or create the customer
            Customer? customer;
            var existing = await _db.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (existing != null)
            {
                customer = existing;
            }
            else
            {
                customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString())
                };
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
            }

            // Create the order
            var order = new Order
            {
                CustomerID = customer.CustomerID,
                OrderDate = DateTime.UtcNow,
                TotalAmount = model.Total,
                Status = "Pending"
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Create order items + reduce inventory
            foreach (var ci in cart)
            {
                var product = await _db.Products.Include(p => p.Inventory).FirstOrDefaultAsync(p => p.ProductID == ci.ProductID);
                if (product == null) continue;

                _db.OrderItems.Add(new OrderItem
                {
                    OrderID = order.OrderID,
                    ProductID = ci.ProductID,
                    Quantity = ci.Quantity,
                    PricePerUnit = product.Price // capture price at purchase time
                });

                if (product.Inventory != null)
                {
                    product.Inventory.StockLevel = Math.Max(0, product.Inventory.StockLevel - ci.Quantity);
                    product.Inventory.LastUpdated = DateTime.UtcNow;
                }
            }
            await _db.SaveChangesAsync();

            // Create shipping detail
            _db.ShippingDetails.Add(new ShippingDetail
            {
                OrderID = order.OrderID,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                TrackingNumber = null
            });
            await _db.SaveChangesAsync();

            // Clear the cart
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Confirmation", "Orders", new { id = order.OrderID });
        }
    }
}
