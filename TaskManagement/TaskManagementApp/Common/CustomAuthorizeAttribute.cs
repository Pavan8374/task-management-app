using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace TaskManagementApp.Common
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var session = context.HttpContext.Session;

            // Check if user is authenticated via Identity or session
            bool isAuthenticated = user.Identity.IsAuthenticated ||
                                  !string.IsNullOrEmpty(session.GetString("UserId"));

            if (!isAuthenticated)
            {
                // Redirect to login page
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            // Optional: Add user info to context for easy access
            if (!user.Identity.IsAuthenticated && !string.IsNullOrEmpty(session.GetString("UserId")))
            {
                // Create a custom identity for session-based authentication
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, session.GetString("UserId")),
                new Claim(ClaimTypes.Email, session.GetString("UserEmail") ?? ""),
                new Claim(ClaimTypes.Role, session.GetString("UserRole") ?? "")
            };

                var identity = new ClaimsIdentity(claims, "Session");
                context.HttpContext.User = new ClaimsPrincipal(identity);
            }
        }
    }
}
