using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingChangeDateTimeException: Exception
{
    public BookingChangeDateTimeException() { }
    public BookingChangeDateTimeException(string message) : base(message) { }
    public BookingChangeDateTimeException(string message, Exception ex) : base(message, ex) { }
    protected BookingChangeDateTimeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}