using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingCreateException: Exception
{
    public BookingCreateException() { }
    public BookingCreateException(string message) : base(message) { }
    public BookingCreateException(string message, Exception ex) : base(message, ex) { }
    protected BookingCreateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}