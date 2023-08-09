using System.Runtime.Serialization;

namespace Portal.Services.InventoryServices.Exceptions;

[Serializable]
public class InventoryUpdateException: Exception
{
    public InventoryUpdateException() { }
    public InventoryUpdateException(string message) : base(message) { }
    public InventoryUpdateException(string message, Exception ex) : base(message, ex) { }

    protected InventoryUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}