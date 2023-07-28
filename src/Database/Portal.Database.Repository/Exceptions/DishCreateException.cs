using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

[Serializable]
public class DishCreateException : Exception
{
    public DishCreateException()
    {
    }

    public DishCreateException(string? message) : base(message)
    {
    }

    public DishCreateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DishCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}