using System.Runtime.Serialization;

namespace Portal.Services.ZoneService.Exceptions;

[Serializable]
public class ZoneExistsInventoryException: Exception
{
    public ZoneExistsInventoryException() { }
    public ZoneExistsInventoryException(string message) : base(message) { }
    public ZoneExistsInventoryException(string message, Exception ex) : base(message, ex) { }
    protected ZoneExistsInventoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}