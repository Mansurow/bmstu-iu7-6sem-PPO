using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.IRepositories;

public interface IFeedbackRepository
{
    Task<List<FeedbackDbModel>> GetAllFeedbackByRoomAsync(int roomId);
    Task<List<FeedbackDbModel>> GetAllFeedbackUserAsync(int userId);
    Task<List<FeedbackDbModel>> GetAllFeedbackAsync();
    Task<FeedbackDbModel> GetFeedbackAsync(int feedbackId);
    Task InsertFeedbackAsync(FeedbackDbModel feedback);
    Task UpdateFeedbackAsync(FeedbackDbModel feedback);
    Task DeleteFeedbackAsync(int feedbackId);
}
