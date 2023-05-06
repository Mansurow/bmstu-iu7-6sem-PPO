using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions
{
    [Serializable]
    public class BookingDeleteException : Exception
    {
        public BookingDeleteException()
        {
        }

        public BookingDeleteException(string? message) : base(message)
        {
        }

        public BookingDeleteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookingDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}