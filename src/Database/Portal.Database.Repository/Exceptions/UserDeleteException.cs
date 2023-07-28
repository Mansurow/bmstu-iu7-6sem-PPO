using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

[Serializable]
public class UserDeleteException : Exception
{
    public UserDeleteException()
    {
    }

    public UserDeleteException(string? message) : base(message)
    {
    }

    public UserDeleteException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UserDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}