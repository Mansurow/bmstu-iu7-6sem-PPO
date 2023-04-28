namespace Anticafe.BL.Exceptions
{
    public class IncorrectPasswordException: Exception
    {
        public IncorrectPasswordException() { }
        public IncorrectPasswordException(string message) : base(message) { }
        public IncorrectPasswordException(string message, Exception ex) : base(message, ex) { }
    }
}
