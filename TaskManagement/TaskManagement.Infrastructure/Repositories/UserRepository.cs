using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;

namespace TaskManagement.Infrastructure.Repositories
{
    /// <summary>
    /// User repository
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<PagedResult<UserViewModel>> GetUsersAsync(
            int page,
            int pageSize,
            string? search,
            string? sortBy,
            bool ascending)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            // Searching
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.UserName.Contains(search) ||
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search) ||
                    u.Email.Contains(search));
            }

            // Sorting
            query = sortBy?.ToLower() switch
            {
                "firstname" => ascending ? query.OrderBy(u => u.FirstName) : query.OrderByDescending(u => u.FirstName),
                "lastname" => ascending ? query.OrderBy(u => u.LastName) : query.OrderByDescending(u => u.LastName),
                "email" => ascending ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
                "createdat" => ascending ? query.OrderBy(u => u.CreatedAt) : query.OrderByDescending(u => u.CreatedAt),
                _ => ascending ? query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id)
            };

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfileImage = u.ProfileImage,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<UserViewModel>
            {
                Items = users,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }

        public async Task<UserStatsModel> GetUserStatsAsync(UserQueryRequest request)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(u =>
                    u.UserName.Contains(request.Search) ||
                    u.Email.Contains(request.Search) ||
                    u.FirstName.Contains(request.Search) ||
                    u.LastName.Contains(request.Search));
            }

            if (request.UserId > 0)
            {
                query = query.Where(u => u.Id == request.UserId);
            }

            // Get current month for new users calculation
            var now = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var taskstats = await _context.Tasks
                .AsNoTracking()
                .Where(t => !t.IsDeleted)
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
            return new UserStatsModel
            {
                TotalUsers = await query.CountAsync(),
                ActiveUsers = await query.Where(u => u.IsActive).CountAsync(),
                InactiveUsers = await query.Where(u => !u.IsActive).CountAsync(),
                TotalTasks  = taskstats.TotalTasks,
                CompletedTasks = taskstats.CompletedTasks,
                PendingTasks = taskstats.PendingTasks,
            };
        }

        //public async Task<PagedResult<UserViewModel>> GetUsersAsync(int page, int pageSize, string search, string sortBy, bool isAsc)
        //{
        //    var query = _context.Users.AsNoTracking().AsQueryable();

        //    // Apply search filter
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(u =>
        //            u.UserName.Contains(search) ||
        //            u.Email.Contains(search) ||
        //            u.FirstName.Contains(search) ||
        //            u.LastName.Contains(search));
        //    }

        //    // Apply sorting
        //    query = sortBy switch
        //    {
        //        "Email" => isAsc ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
        //        "CreatedAt" => isAsc ? query.OrderBy(u => u.CreatedAt) : query.OrderByDescending(u => u.CreatedAt),
        //        _ => isAsc ? query.OrderBy(u => u.UserName) : query.OrderByDescending(u => u.UserName)
        //    };

        //    // Get total count before pagination
        //    var totalCount = await query.CountAsync();

        //    // Apply pagination
        //    var users = await query
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(u => new UserViewModel
        //        {
        //            Id = u.Id,
        //            UserName = u.UserName,
        //            Email = u.Email,
        //            FirstName = u.FirstName,
        //            LastName = u.LastName,
        //            IsActive = u.IsActive,
        //            CreatedAt = u.CreatedAt
        //        })
        //        .ToListAsync();

        //    return new PagedResult<UserViewModel>
        //    {
        //        Items = users,
        //        Page = page,
        //        PageSize = pageSize,
        //        TotalCount = totalCount
        //    };
        //}

        public async Task<bool> ManageUser(int userId, bool isActive)
        {
            try
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                    throw new ArgumentException("User not found");

                user.IsActive = isActive;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
