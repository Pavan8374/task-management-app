using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.Tasks
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string TaskStatus { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public string TaskPriority { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string UserName { get; set; }
        public List<TaskImageViewModel> TaskImages { get; set; } = new List<TaskImageViewModel>();
    }
}
