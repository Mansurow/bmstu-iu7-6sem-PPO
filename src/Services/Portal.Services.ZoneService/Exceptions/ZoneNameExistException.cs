using System.Runtime.Serialization;

namespace Portal.Services.ZoneService.Exceptions;

[Serializable]
public class ZoneNameExistException: Exception
{
    public ZoneNameExistException() { }
    public ZoneNameExistException(string message) : base(message) { }
    public ZoneNameExistException(string message, Exception ex) : base(message, ex) { }
    protected ZoneNameExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
