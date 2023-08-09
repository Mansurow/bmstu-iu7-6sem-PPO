using System.Runtime.Serialization;

namespace Portal.Services.FeedbackService.Exceptions;

[Serializable]
public class FeedbackRemoveException: Exception
{
    public FeedbackRemoveException() { }
    public FeedbackRemoveException(string message) : base(message) { }
    public FeedbackRemoveException(string message, Exception ex) : base(message, ex) { }
    protected FeedbackRemoveException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}