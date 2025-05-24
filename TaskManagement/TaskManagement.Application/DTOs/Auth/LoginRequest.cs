namespace TaskManagement.Application.DTOs.Auth
{
    /// <summary>
    /// Auth request model
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
