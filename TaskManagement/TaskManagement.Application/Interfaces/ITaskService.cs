using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Models.Tasks;
using TaskManagement.Application.Tasks;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskViewModel> GetTaskByIdAsync(int id);
        Task<List<TaskViewModel>> GetUserTasksAsync(int userId);
        Task<TaskViewModel> CreateTaskAsync(CreateTaskRequest createTaskRequest, int userId);
        Task<TaskViewModel> UpdateTaskAsync(UpdateTaskRequest createTaskRequest, int userId);
        Task<TaskViewModel> DeleteTaskAsync(int taskId, int userId);
        Task<List<TaskViewModel>> GetFilteredTasksAsync(int userId, TaskFilterRequest taskFilterRequest);
        Task<TaskViewModel> UpdateTaskStatusAsync(int taskId, string taskStatus, int userId);
    }
}
