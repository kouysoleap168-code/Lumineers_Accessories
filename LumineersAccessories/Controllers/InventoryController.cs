using Microsoft.AspNetCore.Mvc;

namespace LumineersAccessories.Controllers
{
    /// <summary>Redirects to the Admin area inventory management page.</summary>
    public class InventoryController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Inventory", new { area = "Admin" });
    }
}
