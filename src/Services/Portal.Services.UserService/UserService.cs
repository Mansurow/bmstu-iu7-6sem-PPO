using Portal.Database.Repositories.Interfaces;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Services.UserService.Exceptions;

namespace Portal.Services.UserService;

/// <summary>
/// Сервис пользователя
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }
    
    public Task<List<User>> GetAllUsersAsync()
    {
        return _userRepository.GetAllUsersAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            
            return user;
        }
        catch (Exception)
        {
            throw new UserNotFoundException($"User not found with id: {userId}");
        }
    }
    
    public async Task ChangeUserPermissionsAsync(Guid userId, Role permissions)
    {
        var user = await GetUserByIdAsync(userId);
        try
        {
            user.ChangePermission(permissions);
            await _userRepository.UpdateUserAsync(user);
        }
        catch (Exception)
        {
            throw new UserUpdateException($"User's {userId} permissions have not been changed");
        }
    }
}
