using System.Runtime.Serialization;

namespace Portal.Services.ZoneService.Exceptions;

[Serializable]
public class ZonePackageExistsException: Exception
{
    public ZonePackageExistsException() { }
    public ZonePackageExistsException(string message) : base(message) { }
    public ZonePackageExistsException(string message, Exception ex) : base(message, ex) { }
    protected ZonePackageExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}