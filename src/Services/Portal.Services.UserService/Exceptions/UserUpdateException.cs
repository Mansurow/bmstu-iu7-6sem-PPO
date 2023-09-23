using System.Runtime.Serialization;

namespace Portal.Services.UserService.Exceptions;

[Serializable]
public class UserUpdateException : Exception
{
    public UserUpdateException()
    {
    }

    public UserUpdateException(string? message) : base(message)
    {
    }

    public UserUpdateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UserUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}