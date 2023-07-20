using Portal.Common.Models;

namespace Portal.Services.FeedbackService
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllFeedbackByZoneAsync(Guid zoneId);
        Task<List<Feedback>> GetAllFeedbackAsync();
        Task AddFeedbackAsync(Guid zoneId, Guid userId, double mark, string description);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task UpdateZoneRatingAsync(Guid zoneId);
        Task RemoveFeedbackAsync(Guid feedbackId);
    }
}
