namespace Anticafe.DataAccess.Exceptions;

public class FeedbackUpdateException: Exception
{
    public FeedbackUpdateException() { }
    public FeedbackUpdateException(string message) : base(message) { }
    public FeedbackUpdateException(string message, Exception ex) : base(message, ex) { }
}
