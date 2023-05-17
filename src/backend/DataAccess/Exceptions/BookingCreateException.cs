using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

[Serializable]
public class BookingCreateException : Exception
{
    public BookingCreateException() { }
    public BookingCreateException(string? message) : base(message) { }
    public BookingCreateException(string? message, Exception? innerException) : base(message, innerException) { }
    protected BookingCreateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}