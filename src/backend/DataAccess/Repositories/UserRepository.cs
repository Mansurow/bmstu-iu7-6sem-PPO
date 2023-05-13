using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Anticafe.DataAccess.Repositories
{
    public class UserRepository: BaseRepository, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(IDbContextFactory contextFactory) : base()
        {
            _context = contextFactory.getDbContext();
        }

        public async Task<List<UserDbModel>> GetAllUsersAsync()
        {
                var users = await _context.Users.ToListAsync();
                return users;
        }

        public async Task<UserDbModel> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) 
            {
                throw new UserNotFoundByIdException($"User not found with id: {userId}");
            }
            return user;
        }

        public async Task<UserDbModel> GetUserByEmailAsync(string email) 
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            /*if (user is null)
            {
                throw new UserNotFoundByEmailException($"User not found by email: {email}");
            }*/
            return user;
        }

        public async Task InsertUserAsync(UserDbModel createUser) 
        {
            try
            {
                if (_context.Users.Count() > 0)
                    createUser.Id = _context.Users.Select(x => x.Id).Max() + 1;
                else
                    createUser.Id = 1;

                _context.Users.Add(createUser);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new UserCreateException("User not create");
            }
        }

        public async Task UpdateUserAsync(UserDbModel updateUser)
        {
            try
            {
                _context.Users.Update(updateUser);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new UserUpdateException($"User not update with id: {updateUser.Id}");
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new UserDeleteException($"User not delete with id: {userId}");
            }
        }
    }
}
