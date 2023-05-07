using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.IRepositories;

namespace Anticafe.BL.Sevices.OauthService
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

            await _userRepository.InsertUserAsync(UserConverter.ConvertAppModelToDbModel(user));
        }

        public async Task<User> LogIn(string login, string password)
        {
            var userDb = await _userRepository.GetUserByEmailAsync(login);
            if (userDb is null)
            {
                throw new UserLoginNotFoundException($"User with login: {login} not found.");
            }

            var user = UserConverter.ConvertDbModelToAppModel(userDb);

            if (!user.VerifyPassword(password))
            {
                throw new IncorrectPasswordException("User password is incorrect.");
            }

            return user;
        }
    }
}
