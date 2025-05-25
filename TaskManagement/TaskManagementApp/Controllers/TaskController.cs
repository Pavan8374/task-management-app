using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Models.Tasks;
using TaskManagement.Application.Tasks;
using TaskManagementApp.Common;
using TaskManagementApp.Models.DashBoard;

namespace TaskManagementApp.Controllers
{
    //[Authorize]
    public class TaskController : BaseController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("Dashboard")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Dashboard(TaskFilterRequestModel model)
        {
            try
            {
                var stats = await _taskService.GetTaskStatsAsync(GetCurrentUserId());
                var tasks = await _taskService.GetFilteredTasksAsync(GetCurrentUserId(), model); // This now returns PagedResult<TaskResponseModel>

                var viewModel = new DashboardViewModel
                {
                    TotalTasks = stats.TotalTasks,
                    CompletedTasks = stats.CompletedTasks,
                    InProgressTasks = stats.InProgressTasks,
                    PendingTasks = stats.PendingTasks,
                    OverdueTasks = stats.OverdueTasks,
                    StatusOptions = new List<string> { "Pending", "In Progress", "Completed", "On Hold" },
                    PriorityOptions = new List<string> { "Low", "Medium", "High", "Critical" },
                    Tasks = tasks, // Assign the PagedResult<TaskResponseModel> directly
                    CurrentFilter = model // Assign the current filter model
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading dashboard: " + ex.Message;
                // Ensure a valid DashboardViewModel is returned even on error
                return View(new DashboardViewModel
                {
                    StatusOptions = new List<string> { "Pending", "In Progress", "Completed", "On Hold" },
                    PriorityOptions = new List<string> { "Low", "Medium", "High", "Critical" }
                });
            }
        }

        [HttpGet]
        [Route("GetTasks")]
        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterRequestModel request)
        {
            try
            {
                //var tasks = GetDummyTasks();

                var result = await _taskService.GetFilteredTasksAsync(GetCurrentUserId(), request);

                var tasks = result.Items;

                return Json(new TaskListResponse
                {
                    Success = true,
                    Data = tasks,
                    TotalCount = tasks.Count,
                    CurrentPage = request?.Page ?? 1,
                    TotalPages = (int)Math.Ceiling(tasks.Count / (double)(request?.PageSize ?? 10))
                });
            }
            catch (Exception ex)
            {
                return Json(new TaskListResponse
                {
                    Success = false,
                    Message = "Error retrieving tasks",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Initialize model with default values if needed
            var model = new CreateTaskRequest
            {
                DueDate = DateTime.Now.AddDays(1).Date.AddHours(9) // Default to tomorrow at 9 AM
            };
            return View(model);
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTaskRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Return the view with validation errors
                    return View(request);
                }
                var task = await _taskService.CreateTaskAsync(request, GetCurrentUserId());

                TempData["SuccessMessage"] = "Task created successfully!";

                return RedirectToAction("Dashboard", "Task");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the task. Please try again.";
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            // Initialize model with default values if needed
            var task = await _taskService.GetTaskByIdAsync(id);
            var response = new UpdateTaskRequest()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                TaskStatus = task.TaskStatus,
                TaskPriority = task.TaskPriority
            };
            return View(response);
        }

        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTaskRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                await _taskService.UpdateTaskAsync(request, GetCurrentUserId());
                TempData["SuccessMessage"] = "Task updated successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating task: " + ex.Message;
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                return View(task);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error retrieving task details: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                return View(task);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading task for deletion: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        // POST: Task/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id, GetCurrentUserId());
                TempData["SuccessMessage"] = "Task deleted successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting task: " + ex.Message;
                return RedirectToAction("Delete", new { id });
            }
        }

        [HttpGet]
        [Route("GetTask/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                //var tasks = GetDummyTasks();
                var task = await _taskService.GetTaskByIdAsync(id);

                if (task == null)
                {
                    return Json(new TaskResponse
                    {
                        Success = false,
                        Message = "Task not found"
                    });
                }

                return Json(new TaskResponse
                {
                    Success = true,
                    Data = task
                });
            }
            catch (Exception ex)
            {
                return Json(new TaskResponse
                {
                    Success = false,
                    Message = "Error retrieving task",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPatch]
        [Route("UpdateStatus/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] string status)
        {
            try
            {
                await _taskService.UpdateTaskStatusAsync(id, status, GetCurrentUserId());
                return Json(new TaskResponse
                {
                    Success = true,
                    Message = "Task status updated successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new TaskResponse
                {
                    Success = false,
                    Message = "Error updating task status",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}