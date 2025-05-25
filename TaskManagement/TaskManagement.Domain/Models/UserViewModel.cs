namespace TaskManagement.Domain.Models
{
    public class UserViewModel
    {
        public int Id { get; set; } 
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ProfileImage { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
