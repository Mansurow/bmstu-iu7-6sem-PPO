using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

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