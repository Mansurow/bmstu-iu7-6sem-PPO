using System.Runtime.Serialization;

namespace Portal.Services.MenuService.Exceptions;

[Serializable]
public class DishNotFoundException : Exception
{
    public DishNotFoundException() {}
    public DishNotFoundException(string? message) : base(message) {}
    public DishNotFoundException(string? message, Exception? innerException) : base(message, innerException) {}
    protected DishNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {}
}