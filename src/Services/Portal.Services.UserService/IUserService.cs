using Portal.Common.Models;

namespace Portal.Sevices.UserService;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(Guid userId);
    Task ChangeUserPermissionsAsync(Guid userId);
}
