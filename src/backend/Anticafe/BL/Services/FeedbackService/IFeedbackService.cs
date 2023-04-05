using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.FeedbackService
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllFeedbackByRoomAsync(int roomId);
        Task AddFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(int feedbackId);
    }
}
