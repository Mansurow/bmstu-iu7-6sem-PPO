using System.Runtime.Serialization;

namespace Anticafe.BL.Exceptions;

[Serializable]
public class FeedbackNotFoundException: Exception
{
    public FeedbackNotFoundException() { }
    public FeedbackNotFoundException(string message) : base(message) { }
    public FeedbackNotFoundException(string message, Exception ex) : base(message, ex) { }
    protected FeedbackNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
