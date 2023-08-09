using System.Runtime.Serialization;

namespace Portal.Services.InventoryServices.Exceptions;

[Serializable]
public class InventoryCreateException: Exception
{
    public InventoryCreateException() { }
    public InventoryCreateException(string message) : base(message) { }
    public InventoryCreateException(string message, Exception ex) : base(message, ex) { }
    protected InventoryCreateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}