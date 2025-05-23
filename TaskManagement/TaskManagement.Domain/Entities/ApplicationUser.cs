using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Domain.Entities
{
    /// <summary>
    /// User detail
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Profile image
        /// </summary>
        public string ProfileImage { get; set; }

        /// <summary>
        /// Is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Created at date
        /// </summary>
        public DateTime CreatedAt { get; set; }
    } 
}
