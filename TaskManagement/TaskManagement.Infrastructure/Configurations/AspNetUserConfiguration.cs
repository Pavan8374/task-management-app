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
        }
    }
}
