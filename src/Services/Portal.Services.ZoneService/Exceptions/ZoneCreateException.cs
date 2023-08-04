using System.Runtime.Serialization;

namespace Portal.Services.ZoneService.Exceptions;

[Serializable]
public class ZoneCreateException: Exception
{
    public ZoneCreateException() { }
    public ZoneCreateException(string message) : base(message) { }
    public ZoneCreateException(string message, Exception ex) : base(message, ex) { }
    protected ZoneCreateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}