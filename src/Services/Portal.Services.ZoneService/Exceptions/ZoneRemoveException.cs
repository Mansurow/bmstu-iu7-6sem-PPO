using System.Runtime.Serialization;

namespace Portal.Services.ZoneService.Exceptions;

[Serializable]
public class ZoneRemoveException: Exception
{
    public ZoneRemoveException() { }
    public ZoneRemoveException(string message) : base(message) { }
    public ZoneRemoveException(string message, Exception ex) : base(message, ex) { }
    protected ZoneRemoveException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}