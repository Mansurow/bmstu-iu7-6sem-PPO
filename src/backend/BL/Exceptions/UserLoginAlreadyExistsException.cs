namespace Anticafe.BL.Exceptions
{
    public class UserLoginAlreadyExistsException: Exception
    {
        public UserLoginAlreadyExistsException() { }
        public UserLoginAlreadyExistsException(string message) : base(message) { }
        public UserLoginAlreadyExistsException(string message, Exception ex) : base(message, ex) { }
    }
}
