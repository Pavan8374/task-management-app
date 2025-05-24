using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Models.Tasks;
using TaskManagement.Application.Tasks;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        public Task<TaskViewModel> CreateTaskAsync(CreateTaskRequest createTaskRequest, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskViewModel> DeleteTaskAsync(int taskId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskViewModel>> GetFilteredTasksAsync(int userId, TaskFilterRequest taskFilterRequest)
        {
            throw new NotImplementedException();
        }

        public Task<TaskViewModel> GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskViewModel>> GetUserTasksAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskViewModel> UpdateTaskAsync(UpdateTaskRequest createTaskRequest, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskViewModel> UpdateTaskStatusAsync(int taskId, string taskStatus, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
