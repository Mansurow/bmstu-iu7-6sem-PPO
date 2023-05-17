using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions
{
    [Serializable]
    public class BookingUpdateException : Exception
    {
        public BookingUpdateException()
        {
        }

        public BookingUpdateException(string? message) : base(message)
        {
        }

        public BookingUpdateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookingUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}