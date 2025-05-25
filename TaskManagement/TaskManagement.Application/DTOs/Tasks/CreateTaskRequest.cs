using Microsoft.AspNetCore.Mvc.Rendering;
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

        //[Required(ErrorMessage = "Status is required")]
        //public string TaskStatus { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public string TaskPriority { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        //public List<SelectListItem> GetStatusOptions()
        //{
        //    // Replace this with your data source
        //    List<SelectListItem> options = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "InProgress", Text = "InProgress" },
        //    new SelectListItem { Value = "Pending", Text = "Pending" },
        //    new SelectListItem { Value = "Completed", Text = "Completed" }
        //};
        //    return options;
        //}

        //public List<SelectListItem> GetPriorityOptions()
        //{
        //    // Replace this with your data source
        //    List<SelectListItem> options = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "Low", Text = "Low" },
        //    new SelectListItem { Value = "Medium", Text = "Medium" },
        //    new SelectListItem { Value = "High", Text = "High" }
        //};
        //    return options;
        //}
    }
}
