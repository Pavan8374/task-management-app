namespace TaskManagement.Application.Authentication
{
    /// <summary>
    /// Auth request model
    /// </summary>
    public class AuthRequest
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
