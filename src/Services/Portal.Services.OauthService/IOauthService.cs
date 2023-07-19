using Portal.Common.Models;

namespace Portal.Sevices.OauthService;

public interface IOauthService
{
    Task Registrate(User user, string password);
    Task<User> LogIn(string login, string password);
}
