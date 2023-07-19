using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

public interface IFeedbackRepository
{
    Task<List<Feedback>> GetAllFeedbackByZoneAsync(Guid zoneId);
    Task<List<Feedback>> GetAllFeedbackUserAsync(Guid userId);
    Task<List<Feedback>> GetAllFeedbackAsync();
    Task<Feedback> GetFeedbackAsync(Guid feedbackId);
    Task InsertFeedbackAsync(Feedback feedback);
    Task UpdateFeedbackAsync(Feedback feedback);
    Task DeleteFeedbackAsync(Guid feedbackId);
}
