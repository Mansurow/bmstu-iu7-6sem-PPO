using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Common.Core;
using Portal.Common.Enums;
using Portal.Database.Core.Repositories;
using Portal.Services.UserService.Exceptions;

namespace Portal.Services.UserService;

/// <summary>
/// Сервис пользователя
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "User with id: {UserId} not found", userId);
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
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "User with id: {UserId} not found", userId);
            throw new UserNotFoundException($"User with id: {userId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while changing permissions for user: {UserId}", userId);
            throw new UserUpdateException($"User's {userId} permissions have not been changed");
        }
    }

    public async Task CreateAdmin(string login, string password)
    {
        try
        {
            var admins = await _userRepository.GetAdmins();

            /*foreach (var admin in admins)
            {
                await _userRepository.DeleteUserAsync(admin.Id);
            }*/
            
            if (admins.Any(admin => admin.Email == login))
            {
                _logger.LogInformation("Administrator already has been created");
                return;
            }

            var user = new User(Guid.NewGuid(), login, "", "",
                DateOnly.FromDateTime(DateTime.UtcNow), Gender.Unknown, login);
            user.ChangePermission(Role.Administrator);
            user.CreateHash(password);
            
            await _userRepository.InsertUserAsync(user);
            _logger.LogInformation("Administrator was created successfully");

        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while creating administrator");
            throw new UserCreateException($"Administrator has not been created");
        }
    }
}
