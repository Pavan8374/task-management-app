using TaskManagement.Application.Models.Tasks;
using TaskManagement.Application.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagementApp.Models.DashBoard
{
    public class DashboardViewModel
    {
        //public List<TaskResponseModel> Tasks { get; set; } = new List<TaskResponseModel>();
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
        public int OverdueTasks { get; set; }
        // New property to hold the paginated tasks
        public PagedResult<TaskResponseModel> Tasks { get; set; } = new PagedResult<TaskResponseModel>();

        // Property to hold the current filter model (useful for retaining filter state on page reload)
        public TaskFilterRequestModel CurrentFilter { get; set; } = new TaskFilterRequestModel();


        // Filter options
        public List<string> StatusOptions { get; set; } = new List<string> { "Pending", "In Progress", "Completed", "On Hold" };
        public List<string> PriorityOptions { get; set; } = new List<string> { "Low", "Medium", "High", "Critical" };
    }
}
