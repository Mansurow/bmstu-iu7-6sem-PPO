using Portal.Common.Models;

namespace Portal.Services.FeedbackService
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllFeedbackByRoomAsync(Guid ZoneId);
        Task<List<Feedback>> GetAllFeedbackAsync();
        Task AddFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task UpdateZoneRatingAsync(Guid zoneId);
        Task DeleteFeedbackAsync(Guid feedbackId);
    }
}
