using System.Runtime.Serialization;

namespace Portal.Services.PackageService.Exceptions;

[Serializable]
public class PackageUpdateException: Exception
{
    public PackageUpdateException() { }
    public PackageUpdateException(string message) : base(message) { }
    public PackageUpdateException(string message, Exception ex) : base(message, ex) { }
    protected PackageUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}