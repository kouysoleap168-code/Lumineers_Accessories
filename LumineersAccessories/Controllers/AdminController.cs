using Microsoft.AspNetCore.Mvc;

namespace LumineersAccessories.Controllers
{
    /// <summary>
    /// Redirects /Admin to the Admin area dashboard.
    /// The actual admin pages live in the Admin area (Areas/Admin).
    /// </summary>
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
}
