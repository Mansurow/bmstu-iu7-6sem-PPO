using System.Runtime.Serialization;

namespace Anticafe.BL.Exceptions;

[Serializable]
public class RoomNameExistException: Exception
{
    public RoomNameExistException() { }
    public RoomNameExistException(string message) : base(message) { }
    public RoomNameExistException(string message, Exception ex) : base(message, ex) { }
    protected RoomNameExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
