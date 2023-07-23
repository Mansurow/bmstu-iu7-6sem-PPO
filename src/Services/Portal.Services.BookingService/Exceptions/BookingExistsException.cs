using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingExistsException : Exception
{
    public BookingExistsException() { }
    public BookingExistsException(string message) : base(message) { }
    public BookingExistsException(string message, Exception ex) : base(message, ex) { }
    protected BookingExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
