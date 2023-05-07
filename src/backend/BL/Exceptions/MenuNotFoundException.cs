using System.Runtime.Serialization;

namespace Anticafe.BL.Exceptions;

[Serializable]
public class MenuNotFoundException : Exception
{
    public MenuNotFoundException()
    {
    }

    public MenuNotFoundException(string? message) : base(message)
    {
    }

    public MenuNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected MenuNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}