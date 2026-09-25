using Microsoft.AspNetCore.Mvc;

namespace LumineersAccessories.Controllers
{
    /// <summary>Redirects to the Admin area customers management page.</summary>
    public class CustomersController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Customers", new { area = "Admin" });
    }
}
