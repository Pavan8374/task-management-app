using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs.Tasks
{
    public class CreateTaskRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public string TaskPriority { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }
    }
}
