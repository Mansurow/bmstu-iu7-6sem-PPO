using System.Runtime.Serialization;

namespace Portal.Services.InventoryServices.Exceptions;

[Serializable]
public class InventoryRemoveException: Exception
{
    public InventoryRemoveException() { }
    public InventoryRemoveException(string message) : base(message) { }
    public InventoryRemoveException(string message, Exception ex) : base(message, ex) { }

    protected InventoryRemoveException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}