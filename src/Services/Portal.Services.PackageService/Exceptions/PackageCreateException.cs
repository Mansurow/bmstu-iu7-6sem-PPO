using System.Runtime.Serialization;

namespace Portal.Services.PackageService.Exceptions;

[Serializable]
public class PackageCreateException: Exception
{
    public PackageCreateException() { }
    public PackageCreateException(string message) : base(message) { }
    public PackageCreateException(string message, Exception ex) : base(message, ex) { }
    protected PackageCreateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}