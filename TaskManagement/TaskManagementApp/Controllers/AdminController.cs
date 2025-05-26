using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagementApp.Common;

namespace TaskManagementApp.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(UserQueryRequest userQuery)
        {
            userQuery.Page = userQuery.Page == 0 ? 1 : userQuery.Page;
            userQuery.PageSize = userQuery.PageSize == 0 ? 10 : userQuery.PageSize;

            var users = await _userService.GetUsersAsync(
                userQuery.Page,
                userQuery.PageSize,
                userQuery.Search,
                userQuery.SortBy,
                userQuery.IsAsc
            );

            var userStats = await _userService.GetUserStatsAsync(userQuery);

            var viewModel = new AdminDashboardViewModel
            {
                Users = users,
                UserStats = userStats,
                CurrentQuery = userQuery
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus([FromBody] ToggleUserStatusModel model)
        {
            try
            {
                var result = await _userService.ManageUser(model.UserId, model.IsActive);

                if (result)
                {
                    return Json(new { success = true, message = "User status updated successfully" });
                }

                return Json(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class ToggleUserStatusModel
        {
            public int UserId { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
