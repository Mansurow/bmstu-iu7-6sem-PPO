using System.Runtime.Serialization;

namespace Portal.Services.ZoneService.Exceptions;

[Serializable]
public class ZoneDeleteException: Exception
{
    public ZoneDeleteException() { }
    public ZoneDeleteException(string message) : base(message) { }
    public ZoneDeleteException(string message, Exception ex) : base(message, ex) { }
    protected ZoneDeleteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}