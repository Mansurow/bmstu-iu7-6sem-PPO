using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

[Serializable]
public class UserNotFoundByEmailException : Exception
{
    public UserNotFoundByEmailException()
    {
    }

    public UserNotFoundByEmailException(string? message) : base(message)
    {
    }

    public UserNotFoundByEmailException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UserNotFoundByEmailException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}