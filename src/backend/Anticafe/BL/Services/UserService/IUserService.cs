using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.UserService
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
    }
}
