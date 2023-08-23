using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Portal.Swagger;

public class AuthorizationConfiguration
{
    /// <summary>
    ///  Секретная строка для генерации токена
    /// </summary>
    public string SecretKey { get; set; }
    
    /// <summary>
    /// Длительность жизни токена в секундах
    /// </summary>
    public int TokenLifeTime { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
    }
}