namespace TaskManagement.Domain.Entities
{
    /// <summary>
    /// Task
    /// </summary>
    public class Task : BaseEntity
    {
        /// <summary>
        /// Task title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Task description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Task status
        /// </summary>
        public string TaskStatus { get; set; }

        /// <summary>
        /// Task priority
        /// </summary>
        public string TaskPriority { get; set; }

        /// <summary>
        /// Task due date
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// User identity
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public ApplicationUser User { get; set; }
        public List<TaskImage> TaskImages { get; set; }
    }
}
