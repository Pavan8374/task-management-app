namespace TaskManagement.Application.Tasks
{
    public class TaskResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TaskViewModel Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
