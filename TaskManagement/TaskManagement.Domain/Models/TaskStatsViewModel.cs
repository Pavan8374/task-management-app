namespace TaskManagement.Domain.Models
{
    public class TaskStatsViewModel
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
        public int OnHoldTasks { get; set; }
        public int OverdueTasks { get; set; }
    }
}
