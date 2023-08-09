using Microsoft.EntityFrameworkCore;
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
        catch (InvalidOperationException)
        {
            throw new UserNotFoundException($"User with id: {userId} not found");
        }
    }
    
    public async Task ChangeUserPermissionsAsync(Guid userId, Role permissions)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            user.ChangePermission(permissions);
            await _userRepository.UpdateUserAsync(user);
        }
        catch (InvalidOperationException)
        {
            throw new UserNotFoundException($"User with id: {userId} not found");
        }
        catch (DbUpdateException)
        {
            throw new UserUpdateException($"User's {userId} permissions have not been changed");
        }
    }
}
