using System.Runtime.Serialization;

namespace Portal.Services.OauthService.Exceptions;

[Serializable]
public class UserLoginNotFoundException: Exception
{
    public UserLoginNotFoundException() { }
    public UserLoginNotFoundException(string message) : base(message) { }
    public UserLoginNotFoundException(string message, Exception ex) : base(message, ex) { }
    protected UserLoginNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
