using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.Common.Enums;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.IRepositories;

namespace Anticafe.BL.Sevices.UserService
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> GetUserByIdAsync(int userId) 
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null)
            {
                throw new UserNotFoundException($"Not found user with id: {userId}");
            }

            return UserConverter.ConvertDbModelToAppModel(user);
        }

        public async Task<List<User>> GetAllUsersAsync() 
        {
            return (await _userRepository.GetAllUsersAsync()).Select(u => UserConverter.ConvertDbModelToAppModel(u)).ToList();
        }

        public async Task ChangeUserPermissionsAsync(int userId) 
        {
            var user = await GetUserByIdAsync(userId);

            if (user is not null)
            {
                user.ChangePermission(UserRole.Admin);
                await _userRepository.UpdateUserAsync(UserConverter.ConvertAppModelToDbModel(user));
            }
            else
            {
                throw new UserNotFoundException($"Not found user with id: {userId}");
            }
        }
    }
}
