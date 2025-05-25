using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Configurations
{
    /// <summary>
    /// Asp net user configuration
    /// </summary>
    public class AspNetUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasColumnName("FirstName")
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasColumnName("LastName")
                .HasMaxLength(50);

            builder.Property(u => u.ProfileImage)
                .HasColumnName("ProfileImage");

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getutcdate()");

            //var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = 1,
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = "b3bcf22f-a542-4f76-984c-1556917acd06",
                ConcurrencyStamp = "b3bcf11f-a542-4y76-984c-1856917acd06",
                FirstName = "Admin",
                LastName = "System",
                IsActive = true,
                ProfileImage = ""
            };

            adminUser.PasswordHash = "AQAAAAIAAYagAAAAEFg6wl1lE8vSchSiLupOe03w0rP+q3mn7YU9Vv6ljcPQxGSTk/ghreTMEbP8f0E7OQ==";

            builder.HasData(adminUser);
        }
    }
}
