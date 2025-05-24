using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Models.Tasks;
using TaskManagement.Application.Tasks;
using TaskManagementApp.Models.DashBoard;

namespace TaskManagementApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("")]
        [Route("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // Generate dummy tasks for design showcase
                var tasks = GetDummyTasks();

                var viewModel = new DashboardViewModel
                {
                    Tasks = tasks,
                    TotalTasks = tasks.Count,
                    CompletedTasks = tasks.Count(t => t.TaskStatus == "Completed"),
                    InProgressTasks = tasks.Count(t => t.TaskStatus == "In Progress"),
                    PendingTasks = tasks.Count(t => t.TaskStatus == "Pending"),
                    OverdueTasks = tasks.Count(t => t.DueDate < DateTime.Now && t.TaskStatus != "Completed"),
                    StatusOptions = new List<string> { "Pending", "In Progress", "Completed", "On Hold" },
                    PriorityOptions = new List<string> { "Low", "Medium", "High", "Critical" }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading dashboard: " + ex.Message;
                return View(new DashboardViewModel());
            }
        }

        [HttpGet]
        [Route("GetTasks")]
        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterRequest request)
        {
            try
            {
                var tasks = GetDummyTasks();

                // Apply filters if provided
                if (!string.IsNullOrEmpty(request.Status))
                {
                    tasks = tasks.Where(t => t.TaskStatus.Equals(request.Status, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(request.Priority))
                {
                    tasks = tasks.Where(t => t.TaskPriority.Equals(request.Priority, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    tasks = tasks.Where(t =>
                        t.Title.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        t.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

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
        public IActionResult Create() => View();

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new TaskResponse
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                // Simulate task creation
                var newTask = new TaskViewModel
                {
                    Id = new Random().Next(1000, 9999),
                    Title = request.Title,
                    Description = request.Description,
                    TaskStatus = "Pending",
                    TaskPriority = request.TaskPriority ?? "Medium",
                    DueDate = request.DueDate,
                    CreatedAt = DateTime.Now,
                    UserId = 1,
                    UserName = "Demo User",
                    IsActive = true,
                    IsDeleted = false
                };

                return Json(new TaskResponse
                {
                    Success = true,
                    Message = "Task created successfully",
                    Data = newTask
                });
            }
            catch (Exception ex)
            {
                return Json(new TaskResponse
                {
                    Success = false,
                    Message = "Error creating task",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new TaskResponse
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                return Json(new TaskResponse
                {
                    Success = true,
                    Message = "Task updated successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new TaskResponse
                {
                    Success = false,
                    Message = "Error updating task",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                return Json(new TaskResponse
                {
                    Success = true,
                    Message = "Task deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new TaskResponse
                {
                    Success = false,
                    Message = "Error deleting task",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet]
        [Route("GetTask/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var tasks = GetDummyTasks();
                var task = tasks.FirstOrDefault(t => t.Id == id);

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

        private List<TaskViewModel> GetDummyTasks()
        {
            return new List<TaskViewModel>
            {
                new TaskViewModel
                {
                    Id = 1,
                    Title = "Complete Website Redesign",
                    Description = "Redesign the company website with modern UI/UX principles. Include responsive design, accessibility features, and performance optimizations.",
                    TaskStatus = "In Progress",
                    TaskPriority = "High",
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedAt = DateTime.Now.AddDays(-10),
                    UserId = 1,
                    UserName = "John Smith",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 2,
                    Title = "Database Migration",
                    Description = "Migrate legacy database to new cloud infrastructure with minimal downtime.",
                    TaskStatus = "Pending",
                    TaskPriority = "Critical",
                    DueDate = DateTime.Now.AddDays(2),
                    CreatedAt = DateTime.Now.AddDays(-5),
                    UserId = 1,
                    UserName = "Sarah Johnson",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 3,
                    Title = "API Documentation Update",
                    Description = "Update API documentation to reflect recent changes and add code examples for better developer experience.",
                    TaskStatus = "Completed",
                    TaskPriority = "Medium",
                    DueDate = DateTime.Now.AddDays(-2),
                    CreatedAt = DateTime.Now.AddDays(-15),
                    UserId = 1,
                    UserName = "Mike Davis",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 4,
                    Title = "Security Audit",
                    Description = "Conduct comprehensive security audit of all systems and applications.",
                    TaskStatus = "Pending",
                    TaskPriority = "High",
                    DueDate = DateTime.Now.AddDays(7),
                    CreatedAt = DateTime.Now.AddDays(-3),
                    UserId = 1,
                    UserName = "Lisa Wilson",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 5,
                    Title = "Team Training Session",
                    Description = "Organize training session for new development tools and methodologies.",
                    TaskStatus = "On Hold",
                    TaskPriority = "Low",
                    DueDate = DateTime.Now.AddDays(14),
                    CreatedAt = DateTime.Now.AddDays(-7),
                    UserId = 1,
                    UserName = "Robert Brown",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 6,
                    Title = "Mobile App Bug Fixes",
                    Description = "Fix critical bugs reported in the mobile application. Priority on crash issues and performance problems.",
                    TaskStatus = "In Progress",
                    TaskPriority = "Critical",
                    DueDate = DateTime.Now.AddDays(1),
                    CreatedAt = DateTime.Now.AddDays(-2),
                    UserId = 1,
                    UserName = "Emily Chen",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 7,
                    Title = "Customer Feedback Analysis",
                    Description = "Analyze recent customer feedback and create actionable insights report.",
                    TaskStatus = "Completed",
                    TaskPriority = "Medium",
                    DueDate = DateTime.Now.AddDays(-5),
                    CreatedAt = DateTime.Now.AddDays(-20),
                    UserId = 1,
                    UserName = "David Wilson",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 8,
                    Title = "Server Maintenance",
                    Description = "Scheduled maintenance for production servers including updates and optimization.",
                    TaskStatus = "Pending",
                    TaskPriority = "Medium",
                    DueDate = DateTime.Now.AddDays(10),
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UserId = 1,
                    UserName = "Alex Thompson",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 9,
                    Title = "Marketing Campaign Launch",
                    Description = "Launch Q4 marketing campaign across all digital platforms.",
                    TaskStatus = "In Progress",
                    TaskPriority = "High",
                    DueDate = DateTime.Now.AddDays(3),
                    CreatedAt = DateTime.Now.AddDays(-8),
                    UserId = 1,
                    UserName = "Jessica Lee",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 10,
                    Title = "Code Review Process",
                    Description = "Review and approve pending code changes in the development branch.",
                    TaskStatus = "Pending",
                    TaskPriority = "Low",
                    DueDate = DateTime.Now.AddDays(6),
                    CreatedAt = DateTime.Now.AddDays(-4),
                    UserId = 1,
                    UserName = "Chris Garcia",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 11,
                    Title = "Payment Gateway Integration",
                    Description = "Integrate new payment gateway with enhanced security features and multiple payment options.",
                    TaskStatus = "In Progress",
                    TaskPriority = "Critical",
                    DueDate = DateTime.Now.AddDays(8),
                    CreatedAt = DateTime.Now.AddDays(-12),
                    UserId = 1,
                    UserName = "Amanda Rodriguez",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 12,
                    Title = "User Interface Improvements",
                    Description = "Implement user interface improvements based on UX research findings.",
                    TaskStatus = "Completed",
                    TaskPriority = "Medium",
                    DueDate = DateTime.Now.AddDays(-7),
                    CreatedAt = DateTime.Now.AddDays(-25),
                    UserId = 1,
                    UserName = "Kevin Martinez",
                    IsActive = true,
                    IsDeleted = false
                },
                // Add some overdue tasks
                new TaskViewModel
                {
                    Id = 13,
                    Title = "Legacy System Cleanup",
                    Description = "Remove deprecated code and optimize legacy system performance.",
                    TaskStatus = "Pending",
                    TaskPriority = "Medium",
                    DueDate = DateTime.Now.AddDays(-3), // Overdue
                    CreatedAt = DateTime.Now.AddDays(-30),
                    UserId = 1,
                    UserName = "Nicole Taylor",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 14,
                    Title = "Client Presentation Prep",
                    Description = "Prepare presentation materials for upcoming client meeting.",
                    TaskStatus = "In Progress",
                    TaskPriority = "High",
                    DueDate = DateTime.Now.AddDays(-1), // Overdue
                    CreatedAt = DateTime.Now.AddDays(-6),
                    UserId = 1,
                    UserName = "Ryan Anderson",
                    IsActive = true,
                    IsDeleted = false
                },
                new TaskViewModel
                {
                    Id = 15,
                    Title = "Backup System Verification",
                    Description = "Verify integrity and functionality of automated backup systems.",
                    TaskStatus = "Completed",
                    TaskPriority = "High",
                    DueDate = DateTime.Now.AddDays(-10),
                    CreatedAt = DateTime.Now.AddDays(-40),
                    UserId = 1,
                    UserName = "Michelle White",
                    IsActive = true,
                    IsDeleted = false
                }
            };
        }

        private int GetCurrentUserId()
        {
            return 1; // Return dummy user ID for design showcase
        }
    }
}