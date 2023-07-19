using Portal.Database.Repositories.Interfaces;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Services.UserService.Exceptions;

namespace Portal.Sevices.UserService;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<User> GetUserByIdAsync(Guid userId) 
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new UserNotFoundException($"Not found user with id: {userId}");
        }

        return user;
    }

    public async Task<List<User>> GetAllUsersAsync() 
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task ChangeUserPermissionsAsync(Guid userId) 
    {
        var user = await GetUserByIdAsync(userId);

        if (user is not null)
        {
            user.ChangePermission(Role.Administrator);
            await _userRepository.UpdateUserAsync(user);
        }
        else
        {
            throw new UserNotFoundException($"Not found user with id: {userId}");
        }
    }
}
