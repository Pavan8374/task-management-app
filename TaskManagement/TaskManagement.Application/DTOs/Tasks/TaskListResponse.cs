using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Tasks
{
    public class TaskListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<TaskResponseModel> Data { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
