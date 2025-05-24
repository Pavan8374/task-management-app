namespace TaskManagement.Application.DTOs.Auth
{
    /// <summary>
    /// Auth response model
    /// </summary>
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
