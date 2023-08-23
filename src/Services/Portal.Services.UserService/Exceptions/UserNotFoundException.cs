using System.Runtime.Serialization;

namespace Portal.Services.UserService.Exceptions;

[Serializable]
public class UserNotFoundException: Exception
{
    public UserNotFoundException() { }
    public UserNotFoundException(string message) : base(message) { }
    public UserNotFoundException(string message, Exception ex) : base(message, ex) { }
    protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
