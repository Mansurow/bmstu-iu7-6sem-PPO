using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.Common.Enums;
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

        public async Task<User> Registrate(string login, string password) 
        {
            var userModel = await _userRepository.GetUserByEmailAsync(login);
            if (userModel is not null)
            {
                throw new UserLoginAlreadyExistsException($"User with login: {login} already exists.");
            }

            var user = new User(1, "", "", "", DateTime.MinValue, Gender.Unknown, login, "", UserRole.User);
            user.CreateHash(password);

            await _userRepository.InsertUserAsync(UserConverter.ConvertAppModelToDbModel(user));

            return user;
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
