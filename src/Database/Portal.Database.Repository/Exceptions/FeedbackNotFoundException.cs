using System.Runtime.Serialization;

namespace Anticafe.DataAccess.Exceptions;

[Serializable]
public class FeedbackNotFoundException : Exception
{
    public FeedbackNotFoundException()
    {
    }

    public FeedbackNotFoundException(string? message) : base(message)
    {
    }

    public FeedbackNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected FeedbackNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}