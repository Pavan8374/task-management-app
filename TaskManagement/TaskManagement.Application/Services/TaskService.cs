using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Models.Tasks;
using TaskManagement.Application.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskViewModel> CreateTaskAsync(CreateTaskRequest request, int userId)
        {
            ValidateCreateTask(request);

            var task = new Domain.Entities.Task
            {
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                TaskStatus = Domain.Enums.TaskStatus.Pending.ToString(),
                TaskPriority = request.TaskPriority,
                DueDate = request.DueDate,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false,
                TaskImages = new List<TaskImage>()
            };

            task.Id = await _taskRepository.CreateTaskAsync(task);

            return MapToViewModel(task);
        }

        public async Task<TaskViewModel> DeleteTaskAsync(int taskId, int userId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null || task.IsDeleted) throw new Exception("Task not found.");
            if (task.UserId != userId) throw new UnauthorizedAccessException();

            task.IsDeleted = true;
            task.ModifiedAt = DateTime.UtcNow;
            await _taskRepository.UpdateTaskAsync(task);

            return MapToViewModel(task);
        }

        public async Task<PagedResult<TaskResponseModel>> GetFilteredTasksAsync(int userId, TaskFilterRequestModel filter)
        {
            var request = new TaskFilterRequest()
            {
                Status = filter.Status,
                Priority = filter.Priority,
                SearchTerm = filter.SearchTerm,
                DueDateFrom = filter.DueDateFrom,
                DueDateTo = filter.DueDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,

                SortBy = "DueDate",           
                SortDirection = "ASC",        
                IncludeOverdue = true,        
                IncludeCompleted = true       

            };
            return await _taskRepository.GetFilteredTasksAsync(userId, request);
        }

        public async Task<TaskViewModel> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null || task.IsDeleted) throw new Exception("Task not found.");
            return MapToViewModel(task);
        }

        public async Task<List<TaskViewModel>> GetUserTasksAsync(int userId)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            return tasks.Where(t => !t.IsDeleted).Select(MapToViewModel).ToList();
        }

        public async Task<TaskViewModel> UpdateTaskAsync(UpdateTaskRequest request, int userId)
        {
            ValidateUpdateTask(request);

            var task = await _taskRepository.GetTaskByIdAsync(request.Id);
            if (task == null || task.IsDeleted) throw new Exception("Task not found.");
            if (task.UserId != userId) throw new UnauthorizedAccessException();

            task.Title = request.Title.Trim();
            task.Description = request.Description?.Trim();
            task.TaskStatus = request.TaskStatus;
            task.TaskPriority = request.TaskPriority;
            task.DueDate = request.DueDate;
            task.ModifiedAt = DateTime.UtcNow;

            await _taskRepository.UpdateTaskAsync(task);
            return MapToViewModel(task);
        }

        public async Task<TaskViewModel> UpdateTaskStatusAsync(int taskId, string status, int userId)
        {
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status is required.");

            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null || task.IsDeleted) throw new Exception("Task not found.");
            if (task.UserId != userId) throw new UnauthorizedAccessException();

            task.TaskStatus = status;
            task.ModifiedAt = DateTime.UtcNow;

            await _taskRepository.UpdateTaskStatusAsync(task.Id, status);

            return MapToViewModel(task);
        }

        public async Task<TaskStatsViewModel> GetTaskStatsAsync(int userId)
        {
            return await _taskRepository.GetTaskStatsAsync(userId);
        }


        #region Private Helpers

        private void ValidateCreateTask(CreateTaskRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Title)) throw new ArgumentException("Title is required.");
            //if (string.IsNullOrWhiteSpace(request.TaskStatus)) throw new ArgumentException("Status is required.");
            if (string.IsNullOrWhiteSpace(request.TaskPriority)) throw new ArgumentException("Priority is required.");
            if (request.DueDate < DateTime.UtcNow.Date) throw new ArgumentException("Due date must be in the future.");
        }

        private void ValidateUpdateTask(UpdateTaskRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Title)) throw new ArgumentException("Title is required.");
            if (string.IsNullOrWhiteSpace(request.TaskStatus)) throw new ArgumentException("Status is required.");
            if (string.IsNullOrWhiteSpace(request.TaskPriority)) throw new ArgumentException("Priority is required.");
            if (request.DueDate < DateTime.UtcNow.Date) throw new ArgumentException("Due date must be in the future.");
        }

        private TaskViewModel MapToViewModel(Domain.Entities.Task task)
        {
            return new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                TaskStatus = task.TaskStatus,
                TaskPriority = task.TaskPriority,
                DueDate = task.DueDate,
                UserId = task.UserId,
                IsDeleted = task.IsDeleted,
                IsActive = task.IsActive,
                CreatedAt = task.CreatedAt,
                ModifiedAt = task.ModifiedAt,
                UserName = task.User?.UserName ?? "N/A",
                TaskImages = task.TaskImages?.Select(img => new Tasks.TaskImageViewModel
                {
                    Id = img.Id,
                    ImagePath = img.FilePath
                }).ToList() ?? new List<Tasks.TaskImageViewModel>()
            };
        }

        #endregion
    }
}
