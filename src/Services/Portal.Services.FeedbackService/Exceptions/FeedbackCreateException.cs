using System.Runtime.Serialization;

namespace Portal.Services.FeedbackService.Exceptions;

[Serializable]
public class FeedbackCreateException: Exception
{
    public FeedbackCreateException() { }
    public FeedbackCreateException(string message) : base(message) { }
    public FeedbackCreateException(string message, Exception ex) : base(message, ex) { }
    protected FeedbackCreateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}