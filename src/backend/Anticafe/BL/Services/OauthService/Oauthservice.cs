using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.OauthService
{
    public class Oauthservice: IOauthService
    {
        private readonly IUserRepository _userRepository;

        Oauthservice(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Registrate(User user, string password) 
        {
            var userModel = await _userRepository.GetUserByEmailAsync(user.Email);
            if (userModel is not null)
            {
                new Exception();
            }

            // Create Hash for password

            await _userRepository.AddUserAsync(user);
        }

        public async Task LogIn(string login, string password)
        {
            var userModel = await _userRepository.GetUserByEmailAsync(login);
            if (userModel is null)
            {
                new Exception();
            }
            // Verify password 
            // Check password error password incorrect
            // 
        }
    }
}
