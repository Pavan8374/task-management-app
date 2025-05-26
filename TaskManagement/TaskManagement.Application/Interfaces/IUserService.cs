using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Interfaces
{
    /// <summary>
    /// User service interface
    /// </summary>
    public interface IUserService
    {
        Task<PagedResult<UserViewModel>> GetUsersAsync(
            int page,
            int pageSize,
            string? search,
            string? sortBy,
            bool ascending);

        Task<UserStatsModel> GetUserStatsAsync(UserQueryRequest request);
        Task<bool> ManageUser(int userId, bool isActive);

        /// <summary>
        /// Check if user exists with phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<bool> IsUserExistsWithPhoneNumber(string phoneNumber);
    }
}
