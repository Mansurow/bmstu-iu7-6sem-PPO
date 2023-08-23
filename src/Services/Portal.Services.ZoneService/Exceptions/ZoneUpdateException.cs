using System.Runtime.Serialization;

namespace Portal.Services.ZoneService.Exceptions;

[Serializable]
public class ZoneUpdateException: Exception
{
    public ZoneUpdateException() { }
    public ZoneUpdateException(string message) : base(message) { }
    public ZoneUpdateException(string message, Exception ex) : base(message, ex) { }
    protected ZoneUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}