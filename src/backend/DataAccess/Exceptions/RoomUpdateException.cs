using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

[Serializable]
public class RoomUpdateException : Exception
{
    public RoomUpdateException()
    {
    }

    public RoomUpdateException(string? message) : base(message)
    {
    }

    public RoomUpdateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected RoomUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}