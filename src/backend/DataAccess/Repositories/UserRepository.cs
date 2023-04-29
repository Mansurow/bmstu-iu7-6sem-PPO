using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;
using DataAccess.DBModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository: BaseRepository, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return new List<User>();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
