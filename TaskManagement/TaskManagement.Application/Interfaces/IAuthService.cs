using TaskManagement.Application.DTOs.Auth;

namespace TaskManagement.Application.Interfaces
{
    /// <summary>
    /// Auth service interface
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponse> SignUpAsync(SignUpRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
