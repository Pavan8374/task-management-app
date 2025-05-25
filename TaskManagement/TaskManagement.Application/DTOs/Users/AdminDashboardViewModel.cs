using TaskManagement.Domain.Models;

namespace TaskManagement.Application.DTOs.Users
{
    public class AdminDashboardViewModel
    {
        public PagedResult<UserViewModel> Users { get; set; }
        public UserStatsModel UserStats { get; set; }
        public UserQueryRequest CurrentQuery { get; set; }
    }

}
