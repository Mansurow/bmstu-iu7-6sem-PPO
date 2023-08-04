using System.Runtime.Serialization;

namespace Portal.Services.UserService.Exceptions;

[Serializable]
public class UserCreateException : Exception
{
    public UserCreateException()
    {
    }

    public UserCreateException(string? message) : base(message)
    {
    }

    public UserCreateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UserCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}