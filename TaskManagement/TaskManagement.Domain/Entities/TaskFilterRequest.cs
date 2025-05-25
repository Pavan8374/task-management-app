namespace TaskManagement.Domain.Entities
{
    public class TaskFilterRequest
    {
        public string Status { get; set; }
        public string Priority { get; set; }
        public string SearchTerm { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string SortBy { get; set; } = "DueDate"; // DueDate, Title, Priority, Status, CreatedAt
        public string SortDirection { get; set; } = "ASC"; // ASC, DESC
        public bool IncludeOverdue { get; set; } = true;
        public bool IncludeCompleted { get; set; } = true;
    }
}
