namespace TaskManagement.Domain.Models
{
    public class UserStatsModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletedTasks { get; set; }
        public PagedResult<UserViewModel> Users { get; set; } = new PagedResult<UserViewModel>();

        // Property to hold the current filter model (useful for retaining filter state on page reload)
        public UserQueryRequest CurrentFilter { get; set; } = new UserQueryRequest();
    }
}
