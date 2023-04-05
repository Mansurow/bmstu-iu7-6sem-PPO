using Anticafe.BL.Models;

namespace Anticafe.BL.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User createUser);
        Task UpdateUserAsync(User updateUser);
        Task DeleteUserAsync(int userId);
    }
}
