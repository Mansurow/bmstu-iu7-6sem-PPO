using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.OauthService;
using Portal.Services.OauthService.Exceptions;

namespace Portal.Sevices.OauthService
{
    public class Oauthservice: IOauthService
    {
        private readonly IUserRepository _userRepository;

        public Oauthservice(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Registrate(User user, string password) 
        {
            var userModel = await _userRepository.GetUserByEmailAsync(user.Email);
            if (userModel is not null)
            {
                throw new UserLoginAlreadyExistsException($"User with login: {user.Email} already exists.");
            }

            user.CreateHash(password);

            await _userRepository.InsertUserAsync(user);
        }

        public async Task<User> LogIn(string login, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(login);
            if (user is null)
            {
                throw new UserLoginNotFoundException($"User with login: {login} not found.");
            }

            if (!user.VerifyPassword(password))
            {
                throw new IncorrectPasswordException("User password is incorrect.");
            }

            return user;
        }
    }
}
