namespace Anticafe.BL.Exceptions
{
    public class FeedbackNotFoundException: Exception
    {
        public FeedbackNotFoundException() { }
        public FeedbackNotFoundException(string message) : base(message) { }
        public FeedbackNotFoundException(string message, Exception ex) : base(message, ex) { }
    }
}
