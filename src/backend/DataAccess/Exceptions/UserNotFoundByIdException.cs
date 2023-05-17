using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Repositories;

[Serializable]
public class UserNotFoundByIdException : Exception
{
    public UserNotFoundByIdException()
    {
    }

    public UserNotFoundByIdException(string? message) : base(message)
    {
    }

    public UserNotFoundByIdException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UserNotFoundByIdException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}