using System.Runtime.Serialization;

namespace Portal.Services.MenuService.Exceptions;

[Serializable]
public class DishDeleteException : Exception
{
    public DishDeleteException()
    {
    }

    public DishDeleteException(string? message) : base(message)
    {
    }

    public DishDeleteException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DishDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}