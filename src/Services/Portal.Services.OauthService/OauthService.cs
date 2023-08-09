using Microsoft.EntityFrameworkCore;
using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.OauthService.Exceptions;
using Portal.Services.UserService.Exceptions;

namespace Portal.Services.OauthService
{
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public class OauthService: IOauthService
    {
        private readonly IUserRepository _userRepository;

        public OauthService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task Registrate(User user, string password) 
        {
            try
            {
                var userModel = await _userRepository.GetUserByEmailAsync(user.Email);

                throw new UserLoginAlreadyExistsException($"User with login: {userModel.Email} already exists.");
            }
            catch (InvalidOperationException)
            {
                user.CreateHash(password);

                await _userRepository.InsertUserAsync(user);
            }
            catch (DbUpdateException)
            {
                throw new UserCreateException();
            }
        }

        public async Task<User> LogIn(string login, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(login);
                
                if (!user.VerifyPassword(password))
                {
                    throw new IncorrectPasswordException("User password is incorrect.");
                }

                return user;
            }
            catch (InvalidOperationException)
            {
                throw new UserLoginNotFoundException($"User with login: {login} not found.");
            }
        }
    }
}
