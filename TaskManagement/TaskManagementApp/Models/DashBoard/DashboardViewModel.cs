using TaskManagement.Application.Tasks;

namespace TaskManagementApp.Models.DashBoard
{
    public class DashboardViewModel
    {
        public List<TaskViewModel> Tasks { get; set; } = new List<TaskViewModel>();
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
        public int OverdueTasks { get; set; }

        // Filter options
        public List<string> StatusOptions { get; set; } = new List<string> { "Pending", "In Progress", "Completed", "On Hold" };
        public List<string> PriorityOptions { get; set; } = new List<string> { "Low", "Medium", "High", "Critical" };
    }
}
