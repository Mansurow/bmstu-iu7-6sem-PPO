using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task ChangeUserPermissionsAsync(int userId);
    }
}
