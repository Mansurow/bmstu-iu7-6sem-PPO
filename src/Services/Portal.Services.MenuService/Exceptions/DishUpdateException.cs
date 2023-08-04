using System.Runtime.Serialization;

namespace Portal.Services.MenuService.Exceptions;

[Serializable]
public class DishUpdateException : Exception
{
    public DishUpdateException()
    {
    }

    public DishUpdateException(string? message) : base(message)
    {
    }

    public DishUpdateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DishUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}