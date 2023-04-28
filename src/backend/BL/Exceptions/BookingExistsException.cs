namespace Anticafe.BL.Exceptions
{
    public class BookingExistsException: Exception
    {
        public BookingExistsException() { }
        public BookingExistsException(string message) : base(message) { }
        public BookingExistsException(string message, Exception ex) : base(message, ex) { }
    }
}
