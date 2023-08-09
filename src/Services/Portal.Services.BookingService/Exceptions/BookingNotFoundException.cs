using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingNotFoundException : Exception
{
    public BookingNotFoundException() { }
    public BookingNotFoundException(string message) : base(message) { }
    public BookingNotFoundException(string message, Exception ex) : base(message, ex) { }
    protected BookingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
