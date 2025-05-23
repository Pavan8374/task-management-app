namespace TaskManagement.Domain.Entities
{
    /// <summary>
    /// Task image
    /// </summary>
    public class TaskImage
    {
        /// <summary>
        /// Task image identity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Task identity
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Task filepath or image url
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Image uploaded date
        /// </summary>
        public DateTime UploadedAt { get; set; }

        public Task Task { get; set; }
    }
}
