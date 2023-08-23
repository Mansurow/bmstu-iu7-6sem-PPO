using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingReversedException : Exception
{
    public BookingReversedException() { }
    public BookingReversedException(string message) : base(message) { }
    public BookingReversedException(string message, Exception ex) : base(message, ex) { }
    protected BookingReversedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
