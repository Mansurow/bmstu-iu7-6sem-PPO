using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.OauthService
{
    public interface IOauthService
    {
        Task<User> Registrate(string login, string password);
        Task<User> LogIn(string login, string password);
    }
}
