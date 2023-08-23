using System.Runtime.Serialization;

namespace Portal.Services.OauthService.Exceptions;

[Serializable]
public class IncorrectPasswordException: Exception
{
    public IncorrectPasswordException() { }
    public IncorrectPasswordException(string message) : base(message) { }
    public IncorrectPasswordException(string message, Exception ex) : base(message, ex) { }
    protected IncorrectPasswordException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
