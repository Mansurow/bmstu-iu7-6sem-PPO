using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingExceedsLimitException: Exception
{
    public BookingExceedsLimitException() { }
    public BookingExceedsLimitException(string message) : base(message) { }
    public BookingExceedsLimitException(string message, Exception ex) : base(message, ex) { }
    protected BookingExceedsLimitException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}