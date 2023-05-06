using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions
{
    [Serializable]
    public class RoomCreateException : Exception
    {
        public RoomCreateException()
        {
        }

        public RoomCreateException(string? message) : base(message)
        {
        }

        public RoomCreateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RoomCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}