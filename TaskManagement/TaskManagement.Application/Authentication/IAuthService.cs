namespace TaskManagement.Application.Authentication
{
    /// <summary>
    /// Auth service interface
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(AuthRequest request);
    }
}
