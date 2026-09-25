using Microsoft.AspNetCore.Mvc;

namespace LumineersAccessories.Controllers
{
    /// <summary>Redirects to the Admin area shipping management page.</summary>
    public class ShippingController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Shipping", new { area = "Admin" });
    }
}
