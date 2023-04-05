using Anticafe.BL.Models;

namespace Anticafe.BL.IRepositories;

public interface IFeedbackRepository
{
    Task<List<Feedback>> GetAllFeedbackByRoomAsync(int roomId);
    Task<List<Feedback>> GetAllFeedbackUserAsync(int userId);
    Task<List<Feedback>> GetFeedbackByUserAsync(int roomId, int userId);
    Task<Feedback> GetFeedbackAsync(int feedbackId);
    Task InsertFeedbackAsync(Feedback feedback);
    Task UpdateFeedbackAsync(Feedback feedback);
    Task DeleteFeedbackByFeedbackIdAsync(int feedbackId);
}
