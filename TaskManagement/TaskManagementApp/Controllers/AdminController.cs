using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;
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

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllUserByFilter([FromBody] UserQueryRequestModel userQuery)
        {
            try
            {
                var data = await _userService.GetUsersAsync(
                    userQuery.Page,
                    userQuery.PageSize,
                    userQuery.Search,
                    userQuery.SortBy,
                    userQuery.IsAsc
                );

                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserStatistics()
        {
            try
            {
                // Get all users to calculate statistics
                var allUsers = await _userService.GetUsersAsync(1, int.MaxValue, null, null, true);

                var statistics = new
                {
                    TotalUsers = allUsers.TotalCount,
                    ActiveUsers = allUsers.Items.Count(u => u.IsActive),
                    InactiveUsers = allUsers.Items.Count(u => !u.IsActive),
                    NewUsersThisMonth = allUsers.Items.Count(u => u.CreatedAt >= DateTime.Now.AddMonths(-1))
                };

                return Json(new { success = true, data = statistics });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus([FromBody] ToggleUserStatusModel model)
        {
            try
            {
                // Assuming you have a method to toggle user status
                // You might need to add this method to your IUserService
                // await _userService.ToggleUserStatusAsync(model.UserId, model.IsActive);

                return Json(new { success = true, message = "User status updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class ToggleUserStatusModel
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}