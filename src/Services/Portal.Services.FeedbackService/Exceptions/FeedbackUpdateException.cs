using System.Runtime.Serialization;

namespace Portal.Services.FeedbackService.Exceptions;

[Serializable]
public class FeedbackUpdateException: Exception
{
    public FeedbackUpdateException() { }
    public FeedbackUpdateException(string message) : base(message) { }
    public FeedbackUpdateException(string message, Exception ex) : base(message, ex) { }
    protected FeedbackUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}