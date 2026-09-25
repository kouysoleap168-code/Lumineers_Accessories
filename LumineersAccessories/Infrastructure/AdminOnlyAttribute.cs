using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LumineersAccessories.Infrastructure
{
    /// <summary>
    /// Simple academic authorization: a customer is an "admin" if their session
    /// email matches the demo admin account. Real projects should use
    /// ASP.NET Core Identity + role-based authorization.
    /// </summary>
    public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var email = context.HttpContext.Session.GetString("CustomerEmail");
            if (string.IsNullOrEmpty(email) || email != "admin@lumineersaccessories.com")
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
            }
        }
    }
}
