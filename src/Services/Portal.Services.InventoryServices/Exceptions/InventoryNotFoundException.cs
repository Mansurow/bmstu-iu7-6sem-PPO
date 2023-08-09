using System.Runtime.Serialization;

namespace Portal.Services.InventoryServices.Exceptions;

[Serializable]
public class InventoryNotFoundException: Exception
{
    public InventoryNotFoundException() { }
    public InventoryNotFoundException(string message) : base(message) { }
    public InventoryNotFoundException(string message, Exception ex) : base(message, ex) { }
    protected InventoryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}