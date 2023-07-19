using Portal.Common.Models;

namespace Portal.Services.UserService;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(Guid userId);
    Task ChangeUserPermissionsAsync(Guid userId);
}
