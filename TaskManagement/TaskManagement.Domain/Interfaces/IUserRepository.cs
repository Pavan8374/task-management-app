using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Interfaces
{
    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get users async 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <param name="sortBy"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        Task<PagedResult<UserViewModel>> GetUsersAsync(
            int page,
            int pageSize,
            string? search,
            string? sortBy,
            bool ascending);

        /// <summary>
        /// Get user stats 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
