using System.Runtime.Serialization;

namespace Portal.Services.MenuService.Exceptions;

[Serializable]
public class DishRemoveException : Exception
{
    public DishRemoveException()
    {
    }

    public DishRemoveException(string? message) : base(message)
    {
    }

    public DishRemoveException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DishRemoveException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}