using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.FeedbackService
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllFeedbackByRoomAsync(int roomId);
        Task<List<Feedback>> GetAllFeedbackAsync();
        Task AddFeedbackAsync(int userId, int roomId, int rating, string? msg);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task UpdateRoomRatingAsync(int roomId);
        Task DeleteFeedbackAsync(int feedbackId);
    }
}
