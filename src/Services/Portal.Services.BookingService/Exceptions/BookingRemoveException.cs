using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingRemoveException: Exception
{
    public BookingRemoveException() { }
    public BookingRemoveException(string message) : base(message) { }
    public BookingRemoveException(string message, Exception ex) : base(message, ex) { }
    protected BookingRemoveException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}