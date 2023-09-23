using System.Runtime.Serialization;

namespace Portal.Services.ZoneService.Exceptions;

[Serializable]
public class ZoneNotFoundException: Exception
{
    public ZoneNotFoundException() { }
    public ZoneNotFoundException(string message) : base(message) { }
    public ZoneNotFoundException(string message, Exception ex) : base(message, ex) { }
    protected ZoneNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
