namespace TaskManagement.Application.Authentication
{
    /// <summary>
    /// Auth response model
    /// </summary>
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
