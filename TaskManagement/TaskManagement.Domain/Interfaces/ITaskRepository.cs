using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Domain.Interfaces
{
    /// <summary>
    /// Task repository interface
    /// </summary>
    public interface ITaskRepository
    {
        Task<int> CreateTaskAsync(TaskManagement.Domain.Entities.Task task);
        Task<int> UpdateTaskAsync(TaskManagement.Domain.Entities.Task task);
        Task DeleteTaskAsync(int taskId);
        Task<int> UpdateTaskStatusAsync(int taskId, string status);
        Task<int> UpdateTaskPriorityAsync(int taskId, string priority);

        Task<TaskManagement.Domain.Entities.Task> GetTaskByIdAsync(int taskId);
        Task<List<TaskManagement.Domain.Entities.Task>> GetTasksByUserIdAsync(int userId);
        Task<PagedResult<TaskResponseModel>> GetFilteredTasksAsync(int userId, TaskFilterRequest filter);
        Task<TaskStatsViewModel> GetTaskStatsAsync(int userId);

    }
}
