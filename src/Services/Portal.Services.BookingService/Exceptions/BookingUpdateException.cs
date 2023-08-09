using System.Runtime.Serialization;

namespace Portal.Services.BookingService.Exceptions;

[Serializable]
public class BookingUpdateException: Exception
{
    public BookingUpdateException() { }
    public BookingUpdateException(string message) : base(message) { }
    public BookingUpdateException(string message, Exception ex) : base(message, ex) { }
    protected BookingUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}