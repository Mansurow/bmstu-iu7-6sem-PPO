using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.FeedbackService
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllFeedbackByRoomAsync(int roomId);
        Task<List<Feedback>> GetAllFeedbackAsync();
        Task AddFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task UpdateRoomRatingAsync(int roomId);
        Task DeleteFeedbackAsync(int feedbackId);
    }
}
