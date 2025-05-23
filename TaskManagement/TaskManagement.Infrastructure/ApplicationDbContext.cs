using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagement.Domain.Entities;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infrastructure
{
    /// <summary>
    /// Application db context
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {

        /// <summary>
        /// Tasks
        /// </summary>
        public DbSet<Task> Tasks { get; set; }   

        /// <summary>
        /// User details
        /// </summary>
        public DbSet<ApplicationUser> UserDetails { get; set; }   

        /// <summary>
        /// Application db context xonstructor
        /// </summary>
        /// <param name="options">Db context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext)));
        }
    }
}
