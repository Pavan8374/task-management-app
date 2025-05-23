using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Infrastructure.Configurations
{
    /// <summary>
    /// Asp net role data configuration
    /// </summary>
    public class AspNetRoleDataConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
    {
        /// <summary>
        /// Configure role data
        /// </summary>
        /// <param name="builder">Entity builder for identity role</param>
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            // Seeding roles
            builder.HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "dca709e6-e6b3-496c-95e6-9bca7e4fb2b5" },
                new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "ba51f770-80c6-4bb9-8549-bc8b01ce2c8d" }
            );
        }
    }
}
