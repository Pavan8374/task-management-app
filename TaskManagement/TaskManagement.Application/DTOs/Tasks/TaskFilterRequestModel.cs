namespace TaskManagement.Application.Models.Tasks
{
    public class TaskFilterRequestModel
    {
        public string Status { get; set; }
        public string Priority { get; set; }
        public string SearchTerm { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
