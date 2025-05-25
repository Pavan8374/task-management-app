using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateTaskAsync(Task task)
        {
            var sql = "EXEC sp_CreateTask @p0, @p1, @p2, @p3, @p4, @p5";

            return await _context.Database.ExecuteSqlRawAsync(sql,
                task.Title,
                task.Description,
                task.TaskStatus,
                task.TaskPriority,
                task.DueDate,
                task.UserId);
        }

        public async Task<int> UpdateTaskAsync(Task task)
        {
            var sql = "EXEC sp_UpdateTask @p0, @p1, @p2, @p3, @p4, @p5";

            return await _context.Database.ExecuteSqlRawAsync(sql,
                task.Id,
                task.Title,
                task.Description,
                task.TaskStatus,
                task.TaskPriority,
                task.DueDate);
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int taskId)
        {
            var sql = "EXEC sp_DeleteTask @p0";

            await _context.Database.ExecuteSqlRawAsync(sql, taskId);
        }

        public async Task<int> UpdateTaskStatusAsync(int taskId, string status)
        {
            var sql = "EXEC sp_UpdateTaskStatus @p0, @p1";

            return await _context.Database.ExecuteSqlRawAsync(sql, taskId, status);
        }

        public async Task<int> UpdateTaskPriorityAsync(int taskId, string priority)
        {
            var sql = "EXEC sp_UpdateTaskPriority @p0, @p1";

            return await _context.Database.ExecuteSqlRawAsync(sql, taskId, priority);
        }
        public async Task<Domain.Entities.Task> GetTaskByIdAsync(int taskId)
        {
            return await _context.Tasks.AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.TaskImages)
                .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);
        }

        public async Task<TaskStatsViewModel> GetTaskStatsAsync(int userId)
        {
            var stats = await _context.Tasks
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .GroupBy(t => 1) // Group all records together
                .Select(g => new TaskStatsViewModel
                {
                    TotalTasks = g.Count(),
                    CompletedTasks = g.Count(t => t.TaskStatus == "Completed"),
                    InProgressTasks = g.Count(t => t.TaskStatus == "In Progress"),
                    PendingTasks = g.Count(t => t.TaskStatus == "Pending"),
                    OnHoldTasks = g.Count(t => t.TaskStatus == "On Hold"),
                    OverdueTasks = g.Count(t => t.DueDate < DateTime.Now && t.TaskStatus != "Completed")
                })
                .FirstOrDefaultAsync();

            return stats ?? new TaskStatsViewModel();
        }

        public async Task<List<Domain.Entities.Task>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks.AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.TaskImages)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderBy(t => t.DueDate)
                .ThenBy(t => t.Id)
                .ToListAsync();
        }
        public async Task<PagedResult<TaskResponseModel>> GetFilteredTasksAsync(int userId, TaskFilterRequest filter)
        {
            var query = _context.Tasks.AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.TaskImages)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .AsQueryable();

            // Apply filters
            query = ApplyAdvancedFilters(query, filter);

            // Apply sorting
            query = ApplySorting(query, filter);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination and select only necessary fields
            var tasks = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(t => new TaskResponseModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    TaskStatus = t.TaskStatus,
                    TaskPriority = t.TaskPriority,
                    DueDate = t.DueDate,
                    UserId = t.UserId,
                    IsDeleted = t.IsDeleted,
                    IsActive = !t.IsDeleted && t.DueDate >= DateTime.Now, // Calculate IsActive
                    CreatedAt = t.CreatedAt,
                    ModifiedAt = t.ModifiedAt,
                    UserName = t.User.UserName ?? t.User.Email, // Fallback to email if username is null
                    TaskImages = t.TaskImages.Select(ti => new TaskImageViewModel
                    {
                        Id = ti.Id,
                        TaskId = ti.TaskId,
                        FilePath = ti.FilePath,
                        UploadedAt = ti.UploadedAt
                    }).ToList()
                })
                .ToListAsync();

            return new PagedResult<TaskResponseModel>
            {
                Items = tasks,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize),
                HasNextPage = filter.Page < (int)Math.Ceiling((double)totalCount / filter.PageSize),
                HasPreviousPage = filter.Page > 1
            };
        }

        private IQueryable<Domain.Entities.Task> ApplyAdvancedFilters(IQueryable<Domain.Entities.Task> query, TaskFilterRequest filter)
        {
            // Filter by status
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(t => t.TaskStatus.ToLower() == filter.Status.ToLower());
            }

            // Filter by priority
            if (!string.IsNullOrWhiteSpace(filter.Priority))
            {
                query = query.Where(t => t.TaskPriority.ToLower() == filter.Priority.ToLower());
            }

            // Filter by search term (searches in title and description)
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(searchTerm) ||
                    t.Description.ToLower().Contains(searchTerm));
            }

            // Filter by due date range
            if (filter.DueDateFrom.HasValue)
            {
                query = query.Where(t => t.DueDate >= filter.DueDateFrom.Value.Date);
            }

            if (filter.DueDateTo.HasValue)
            {
                var endDate = filter.DueDateTo.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(t => t.DueDate <= endDate);
            }

            // Filter overdue tasks
            if (!filter.IncludeOverdue)
            {
                query = query.Where(t => t.DueDate >= DateTime.Now.Date);
            }

            // Filter completed tasks
            if (!filter.IncludeCompleted)
            {
                query = query.Where(t => t.TaskStatus.ToLower() != "completed");
            }

            return query;
        }

        private IQueryable<Domain.Entities.Task> ApplySorting(IQueryable<Domain.Entities.Task> query, TaskFilterRequest filter)
        {
            var isDescending = filter.SortDirection?.ToUpper() == "DESC";

            return filter.SortBy?.ToLower() switch
            {
                "title" => isDescending
                    ? query.OrderByDescending(t => t.Title)
                    : query.OrderBy(t => t.Title),
                "priority" => isDescending
                    ? query.OrderByDescending(t => t.TaskPriority)
                    : query.OrderBy(t => t.TaskPriority),
                "status" => isDescending
                    ? query.OrderByDescending(t => t.TaskStatus)
                    : query.OrderBy(t => t.TaskStatus),
                "createdat" => isDescending
                    ? query.OrderByDescending(t => t.CreatedAt)
                    : query.OrderBy(t => t.CreatedAt),
                _ => isDescending
                    ? query.OrderByDescending(t => t.DueDate).ThenByDescending(t => t.Id)
                    : query.OrderBy(t => t.DueDate).ThenBy(t => t.Id)
            };
        }
    }
}
