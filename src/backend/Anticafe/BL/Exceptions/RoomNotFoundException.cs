namespace Anticafe.BL.Exceptions;

public class RoomNotFoundException: Exception
{
    public RoomNotFoundException() { }
    public RoomNotFoundException(string message) : base(message) { }
    public RoomNotFoundException(string message, Exception ex) : base(message, ex) { }

}
