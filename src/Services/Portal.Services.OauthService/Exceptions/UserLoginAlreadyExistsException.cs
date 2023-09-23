using System.Runtime.Serialization;

namespace Portal.Services.OauthService.Exceptions;

[Serializable]
public class UserLoginAlreadyExistsException: Exception
{
    public UserLoginAlreadyExistsException() { }
    public UserLoginAlreadyExistsException(string message) : base(message) { }
    public UserLoginAlreadyExistsException(string message, Exception ex) : base(message, ex) { }
    protected UserLoginAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
