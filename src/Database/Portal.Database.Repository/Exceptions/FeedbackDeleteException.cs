using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

[Serializable]
public class FeedbackDeleteException: Exception
{
    public FeedbackDeleteException() { }
    public FeedbackDeleteException(string message) : base(message) { }
    public FeedbackDeleteException(string message, Exception ex) : base(message, ex) { }
    protected FeedbackDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
