namespace Anticafe.BL.Exceptions
{
    public class UserLoginNotFoundException: Exception
    {
        public UserLoginNotFoundException() { }
        public UserLoginNotFoundException(string message) : base(message) { }
        public UserLoginNotFoundException(string message, Exception ex) : base(message, ex) { }
    }
}
