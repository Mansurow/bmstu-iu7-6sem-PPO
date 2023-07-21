using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingNotSuitableStatusException: Exception
{
    public BookingNotSuitableStatusException() { }
    public BookingNotSuitableStatusException(string message) : base(message) { }
    public BookingNotSuitableStatusException(string message, Exception ex) : base(message, ex) { }
    protected BookingNotSuitableStatusException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}