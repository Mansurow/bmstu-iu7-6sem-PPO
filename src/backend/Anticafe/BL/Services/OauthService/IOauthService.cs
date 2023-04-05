using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.OauthService
{
    public interface IOauthService
    {
        Task Registrate(User user, string password);
        Task LogIn(string login, string password);
        // Task LogOut();
    }
}
