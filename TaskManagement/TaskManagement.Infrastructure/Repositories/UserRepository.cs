using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Repositories
{
    /// <summary>
    /// User repository
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        //public async Task<int> CreateUser()
        //{
        //    await _context.Database.ExecuteSqlRawAsync(
        //         "EXEC sp_Add @Name = {0}, @Age = {1}, @Department = {2}",
        //         name, age, department);
        //}
    }
}
