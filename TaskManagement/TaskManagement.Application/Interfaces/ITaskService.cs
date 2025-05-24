using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> GetTask(int id);
        Task<List<TaskDto>> GetUserTasksAsync(int userId);
    }
}
