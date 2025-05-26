using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<PagedResult<UserViewModel>> GetUsersAsync(
            int page,
            int pageSize,
            string? search,
            string? sortBy,
            bool ascending)
        {
            return await _userRepository.GetUsersAsync(page, pageSize, search, sortBy, ascending);
        }

        public async Task<UserStatsModel> GetUserStatsAsync(UserQueryRequest request)
        {
            return await _userRepository.GetUserStatsAsync(request);
        }

        public async Task<bool> ManageUser(int userId, bool isActive)
        {
            return await _userRepository.ManageUser(userId, isActive);
        }

        /// <summary>
        /// Check if user exists with phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<bool> IsUserExistsWithPhoneNumber(string phoneNumber)
        {
            return await _userRepository.IsUserExistsWithPhoneNumber(phoneNumber);
        }
    }
}
