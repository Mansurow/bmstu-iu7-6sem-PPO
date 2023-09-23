using System.Runtime.Serialization;

namespace Portal.Services.PackageService.Exceptions;

[Serializable]
public class PackageRemoveException: Exception
{
    public PackageRemoveException() { }
    public PackageRemoveException(string message) : base(message) { }
    public PackageRemoveException(string message, Exception ex) : base(message, ex) { }
    protected PackageRemoveException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}