using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementApp.Common
{
    public static class RoleBasedRoutingHelper
    {
        public static IActionResult RedirectToRoleBasedDashboard(string userRole)
        {
            return userRole?.ToLower() switch
            {
                "admin" => new RedirectToActionResult("Index", "Admin", null),
                "user" => new RedirectToActionResult("Dashboard", "Task", null),
                _ => new RedirectToActionResult("Index", "Home", null)
            };
        }

        public static bool IsAuthorizedForAdminArea(HttpContext context)
        {
            var authToken = context.Session.GetString("AuthToken");
            var userRole = context.Session.GetString("UserRole");

            return !string.IsNullOrEmpty(authToken) && userRole?.ToLower() == "admin";
        }

        public static bool IsAuthenticated(HttpContext context)
        {
            var authToken = context.Session.GetString("AuthToken");
            return !string.IsNullOrEmpty(authToken);
        }
    }

    // Custom attribute for role-based authorization
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredRoles;

        public RoleAuthorizeAttribute(params string[] requiredRoles)
        {
            _requiredRoles = requiredRoles ?? throw new ArgumentNullException(nameof(requiredRoles));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authToken = context.HttpContext.Session.GetString("AuthToken");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(authToken))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (!_requiredRoles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
            {
                // User doesn't have required role, redirect to appropriate dashboard
                context.Result = RoleBasedRoutingHelper.RedirectToRoleBasedDashboard(userRole);
                return;
            }
        }
    }
}
