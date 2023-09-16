using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Common.Models;
using Portal.Database.Core.Repositories;
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
        private readonly ILogger<OauthService> _logger;

        public OauthService(IUserRepository userRepository, ILogger<OauthService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Registrate(User user, string password) 
        {
            try
            {
                var userModel = await _userRepository.GetUserByEmailAsync(user.Email);
                
                _logger.LogError("User with login: {Login} already exists", userModel.Email);
                throw new UserLoginAlreadyExistsException($"User with login: {userModel.Email} already exists.");
            }
            catch (InvalidOperationException)
            {
                _logger.LogInformation("User with login: {Login} not found", user.Email);
                user.CreateHash(password);

                await _userRepository.InsertUserAsync(user);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while creating user");
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
                    _logger.LogError("Password is incorrect for user: {UserId}", user.Id);
                    throw new IncorrectPasswordException($"Password is incorrect for user: {user.Id}");
                }

                return user;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "User with login: {Login} not found", login);
                throw new UserLoginNotFoundException($"User with login: {login} not found.");
            }
        }
    }
}
