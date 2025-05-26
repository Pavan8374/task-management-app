using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace TaskManagementApp.Common
{
    public class BaseController : Controller
    {
        protected string? CurrentUserEmail => HttpContext.Session.GetString("UserEmail");
        protected string? CurrentUserRole => HttpContext.Session.GetString("UserRole");
        protected string? CurrentAuthToken => HttpContext.Session.GetString("AuthToken");
        protected bool IsAuthenticated => !string.IsNullOrEmpty(CurrentAuthToken);

        protected int GetCurrentUserId()
        {
            var userIdClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int claimUserId))
            {
                return claimUserId;
            }

            var sessionUserId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(sessionUserId) && int.TryParse(sessionUserId, out int sessionUserIdInt))
            {
                return sessionUserIdInt;
            }

            return 0; 
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["CurrentUserEmail"] = CurrentUserEmail;
            ViewData["CurrentUserRole"] = CurrentUserRole;
            ViewData["IsAuthenticated"] = IsAuthenticated;

            base.OnActionExecuting(context);
        }

        protected IActionResult RedirectToRoleBasedDashboard()
        {
            return RoleBasedRoutingHelper.RedirectToRoleBasedDashboard(CurrentUserRole);
        }

        protected bool HasRole(string role)
        {
            return CurrentUserRole?.Equals(role, StringComparison.OrdinalIgnoreCase) == true;
        }

        protected bool HasAnyRole(params string[] roles)
        {
            return roles.Any(role => HasRole(role));
        }

        protected IActionResult RequireAuthentication()
        {
            if (!IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }
            return null; 
        }

        protected IActionResult RequireRole(params string[] requiredRoles)
        {
            var authResult = RequireAuthentication();
            if (authResult != null) return authResult;

            if (!HasAnyRole(requiredRoles))
            {
                return RedirectToRoleBasedDashboard();
            }
            return null; 
        }

    }
}