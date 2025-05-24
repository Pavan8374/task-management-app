//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using TaskManagement.Application.Interfaces;

//namespace TaskManagementApp.Controllers
//{
//    public class TaskController : Controller
//    {
//        private readonly ITaskService _taskService;

//        public TaskController(ITaskService taskService)
//        {
//            _taskService = taskService;
//        }

//        [HttpGet]
//        [Route("")]
//        [Route("Dashboard")]
//        public async Task<IActionResult> Dashboard()
//        {
//            try
//            {
//                var userId = GetCurrentUserId();
//                var tasks = await _taskService.GetUserTasksAsync(userId);

//                var viewModel = new DashboardViewModel
//                {
//                    Tasks = tasks,
//                    TotalTasks = tasks.Count,
//                    CompletedTasks = tasks.Count(t => t.TaskStatus == "Completed"),
//                    InProgressTasks = tasks.Count(t => t.TaskStatus == "In Progress"),
//                    PendingTasks = tasks.Count(t => t.TaskStatus == "Pending"),
//                    OverdueTasks = tasks.Count(t => t.DueDate < DateTime.Now && t.TaskStatus != "Completed")
//                };

//                return View(viewModel);
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = "Error loading dashboard: " + ex.Message;
//                return View(new DashboardViewModel());
//            }
//        }

//        [HttpGet]
//        [Route("GetTasks")]
//        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterRequest request)
//        {
//            try
//            {
//                var userId = GetCurrentUserId();
//                var result = await _taskService.GetFilteredTasksAsync(userId, request);

//                return Json(new TaskListResponse
//                {
//                    Success = true,
//                    Data = result.Tasks,
//                    TotalCount = result.TotalCount,
//                    CurrentPage = request.Page,
//                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)request.PageSize)
//                });
//            }
//            catch (Exception ex)
//            {
//                return Json(new TaskListResponse
//                {
//                    Success = false,
//                    Message = "Error retrieving tasks",
//                    Errors = new List<string> { ex.Message }
//                });
//            }
//        }

//        [HttpPost]
//        [Route("Create")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    var errors = ModelState.Values
//                        .SelectMany(v => v.Errors)
//                        .Select(e => e.ErrorMessage)
//                        .ToList();

//                    return Json(new TaskResponse
//                    {
//                        Success = false,
//                        Message = "Validation failed",
//                        Errors = errors
//                    });
//                }

//                var userId = GetCurrentUserId();
//                var task = await _taskService.CreateTaskAsync(request, userId);

//                return Json(new TaskResponse
//                {
//                    Success = true,
//                    Message = "Task created successfully",
//                    Data = task
//                });
//            }
//            catch (Exception ex)
//            {
//                return Json(new TaskResponse
//                {
//                    Success = false,
//                    Message = "Error creating task",
//                    Errors = new List<string> { ex.Message }
//                });
//            }
//        }

//        [HttpPut]
//        [Route("Update")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest request)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    var errors = ModelState.Values
//                        .SelectMany(v => v.Errors)
//                        .Select(e => e.ErrorMessage)
//                        .ToList();

//                    return Json(new TaskResponse
//                    {
//                        Success = false,
//                        Message = "Validation failed",
//                        Errors = errors
//                    });
//                }

//                var userId = GetCurrentUserId();
//                var task = await _taskService.UpdateTaskAsync(request, userId);

//                return Json(new TaskResponse
//                {
//                    Success = true,
//                    Message = "Task updated successfully",
//                    Data = task
//                });
//            }
//            catch (Exception ex)
//            {
//                return Json(new TaskResponse
//                {
//                    Success = false,
//                    Message = "Error updating task",
//                    Errors = new List<string> { ex.Message }
//                });
//            }
//        }

//        [HttpDelete]
//        [Route("Delete/{id}")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteTask(int id)
//        {
//            try
//            {
//                var userId = GetCurrentUserId();
//                await _taskService.DeleteTaskAsync(id, userId);

//                return Json(new TaskResponse
//                {
//                    Success = true,
//                    Message = "Task deleted successfully"
//                });
//            }
//            catch (Exception ex)
//            {
//                return Json(new TaskResponse
//                {
//                    Success = false,
//                    Message = "Error deleting task",
//                    Errors = new List<string> { ex.Message }
//                });
//            }
//        }

//        [HttpGet]
//        [Route("GetTask/{id}")]
//        public async Task<IActionResult> GetTask(int id)
//        {
//            try
//            {
//                var userId = GetCurrentUserId();
//                var task = await _taskService.GetTaskByIdAsync(id, userId);

//                if (task == null)
//                {
//                    return Json(new TaskResponse
//                    {
//                        Success = false,
//                        Message = "Task not found"
//                    });
//                }

//                return Json(new TaskResponse
//                {
//                    Success = true,
//                    Data = task
//                });
//            }
//            catch (Exception ex)
//            {
//                return Json(new TaskResponse
//                {
//                    Success = false,
//                    Message = "Error retrieving task",
//                    Errors = new List<string> { ex.Message }
//                });
//            }
//        }

//        [HttpPatch]
//        [Route("UpdateStatus/{id}")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] string status)
//        {
//            try
//            {
//                var userId = GetCurrentUserId();
//                await _taskService.UpdateTaskStatusAsync(id, status, userId);

//                return Json(new TaskResponse
//                {
//                    Success = true,
//                    Message = "Task status updated successfully"
//                });
//            }
//            catch (Exception ex)
//            {
//                return Json(new TaskResponse
//                {
//                    Success = false,
//                    Message = "Error updating task status",
//                    Errors = new List<string> { ex.Message }
//                });
//            }
//        }

//        private int GetCurrentUserId()
//        {
//            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
//        }
//    }
//}
