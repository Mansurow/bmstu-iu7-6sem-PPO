using System.Runtime.Serialization;

namespace Anticafe.BL.Exceptions;

[Serializable]
public class RoomNotFoundException: Exception
{
    public RoomNotFoundException() { }
    public RoomNotFoundException(string message) : base(message) { }
    public RoomNotFoundException(string message, Exception ex) : base(message, ex) { }
    protected RoomNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
