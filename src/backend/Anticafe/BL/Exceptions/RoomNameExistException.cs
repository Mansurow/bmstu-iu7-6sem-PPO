namespace Anticafe.BL.Exceptions
{
    public class RoomNameExistException: Exception
    {
        public RoomNameExistException() { }
        public RoomNameExistException(string message) : base(message) { }
        public RoomNameExistException(string message, Exception ex) : base(message, ex) { }
    }
}
