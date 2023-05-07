using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.IRepositories;

public interface IUserRepository
{
    Task<List<UserDbModel>> GetAllUsersAsync();
    Task<UserDbModel> GetUserByIdAsync(int userId);
    Task<UserDbModel> GetUserByEmailAsync(string email);
    Task InsertUserAsync(UserDbModel createUser);
    Task UpdateUserAsync(UserDbModel updateUser);
    Task DeleteUserAsync(int userId);
}
