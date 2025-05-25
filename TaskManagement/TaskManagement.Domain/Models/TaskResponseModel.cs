namespace TaskManagement.Domain.Models
{
    public class TaskResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TaskStatus { get; set; }
        public string TaskPriority { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string UserName { get; set; }
        public List<TaskImageViewModel> TaskImages { get; set; } = new List<TaskImageViewModel>();
    }

    public class TaskImageViewModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }

}
