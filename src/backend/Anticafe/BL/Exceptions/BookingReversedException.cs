namespace Anticafe.BL.Exceptions
{
    public class BookingReversedException: Exception
    {
        public BookingReversedException() { }
        public BookingReversedException(string message) : base(message) { }
        public BookingReversedException(string message, Exception ex) : base(message, ex) { }
    }
}
