using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.UserService
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int userId) 
        {
            return (await _userRepository.GetUserByIdAsync(userId));
        }
    }
}
