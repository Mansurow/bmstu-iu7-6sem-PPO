using System.Runtime.Serialization;

namespace Portal.Services.PackageService.Exceptions;

[Serializable]
public class PackageNotFoundException: Exception
{
    public PackageNotFoundException() { }
    public PackageNotFoundException(string message) : base(message) { }
    public PackageNotFoundException(string message, Exception ex) : base(message, ex) { }
    protected PackageNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
