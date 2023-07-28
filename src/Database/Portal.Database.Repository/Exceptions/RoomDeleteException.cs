using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

[Serializable]
public class RoomDeleteException : Exception
{
    public RoomDeleteException()
    {
    }

    public RoomDeleteException(string? message) : base(message)
    {
    }

    public RoomDeleteException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected RoomDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}