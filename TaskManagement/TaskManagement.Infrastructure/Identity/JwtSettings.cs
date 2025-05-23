namespace TaskManagement.Infrastructure.Identity
{
    /// <summary>
    /// JWT settings
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Duration in minutes
        /// </summary>
        public double DurationInMinutes { get; set; }
    }
}
