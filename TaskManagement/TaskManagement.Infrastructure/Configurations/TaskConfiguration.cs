using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Infrastructure.Configurations
{
    /// <summary>
    /// Task configuration
    /// </summary>
    public class TaskConfiguration : IEntityTypeConfiguration<TaskManagement.Domain.Entities.Task>
    {
        public void Configure(EntityTypeBuilder<TaskManagement.Domain.Entities.Task> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasColumnName("Title")
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasColumnName("Description")
                .HasMaxLength(1000);

            builder.Property(t => t.TaskStatus)
                .HasColumnName("TaskStatus")
                .IsRequired()
                .HasDefaultValue(Domain.Enums.TaskStatus.Pending.ToString());

            builder.Property(t => t.TaskPriority)
                .HasColumnName("TaskPriority")
                .IsRequired()
                .HasDefaultValue(Domain.Enums.TaskPriority.Low.ToString());

            builder.Property(t => t.UserId)
                .IsRequired()
                .HasColumnName("UserId");

            builder.Property(t => t.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasColumnType("bit")
                .HasDefaultValue(true);

            builder.Property(t => t.DueDate)
                .HasDefaultValueSql("getutcdate()");

            builder.Property(t => t.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit")
                .HasDefaultValue(true);

            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("getutcdate()");

            builder.Property(t => t.ModifiedAt)
                .HasDefaultValueSql("getutcdate()");

            // Relationships

            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
