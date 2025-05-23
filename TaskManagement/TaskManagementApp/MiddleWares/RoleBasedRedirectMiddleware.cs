using Microsoft.AspNetCore.Identity;

namespace TaskManagementApp.MiddleWares
{
    public class RoleBasedRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleBasedRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserManager<IdentityUser> userManager)
        {
            if (context.User.Identity.IsAuthenticated && context.Request.Path == "/")
            {
                var user = await userManager.GetUserAsync(context.User);
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                    context.Response.Redirect("/admin/dashboard");
                else if (roles.Contains("User"))
                    context.Response.Redirect("/user/dashboard");
            }

            await _next(context);
        }
    }

}
