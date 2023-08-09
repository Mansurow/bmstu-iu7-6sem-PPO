using Microsoft.EntityFrameworkCore;
using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.FeedbackService.Exceptions;
using Portal.Services.UserService.Exceptions;
using Portal.Services.ZoneService.Exceptions;

namespace Portal.Services.FeedbackService;

/// <summary>
/// Сервис отзывов
/// </summary>
public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IZoneRepository _zoneRepository;
    private readonly IUserRepository _userRepository;

    public FeedbackService(IFeedbackRepository feedbackRepository,
                           IZoneRepository zoneRepository,
                           IUserRepository userRepository)
    {
        _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
        _zoneRepository = zoneRepository ?? throw new ArgumentNullException(nameof(zoneRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public Task<List<Feedback>> GetAllFeedbackAsync()
    {
        return  _feedbackRepository.GetAllFeedbackAsync();
    }

    public async Task<List<Feedback>> GetAllFeedbackByZoneAsync(Guid zoneId)
    {
        try
        {
            var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
            
            var feedbacks = await _feedbackRepository.GetAllFeedbackByZoneAsync(zone.Id);

            return feedbacks;
        }
        catch (InvalidOperationException e)
        {
            throw new ZoneNotFoundException($"Zone not found with id: {zoneId}");
        }
    }

    public async Task<Guid> AddFeedbackAsync(Guid zoneId, Guid userId, double mark, string description)
    {
        try
        {
            var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
            if (zone is null)
            {
                throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
            }

            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
            }
            catch (Exception)
            {
                throw new UserNotFoundException($"User with id: {userId} not found");
            }

            var feedback = new Feedback(Guid.NewGuid(), userId, zoneId,
                DateTime.UtcNow, mark, description);
            await _feedbackRepository.InsertFeedbackAsync(feedback);

            return feedback.Id;
        }
        catch (InvalidOperationException e)
        {
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
        }
        catch (DbUpdateException)
        {
            throw new FeedbackCreateException($"Feedback has not been created for user; {userId}");
        }        
    }

    public async Task UpdateZoneRatingAsync(Guid zoneId)
    {
        try
        {
            var allZoneFeedback = await _feedbackRepository.GetAllFeedbackByZoneAsync(zoneId);

            var rating = allZoneFeedback.Sum(feedback => feedback.Mark);
            if (allZoneFeedback.Count > 0)
                rating /= allZoneFeedback.Count;

            await _zoneRepository.UpdateZoneRatingAsync(zoneId, rating);
        }
        catch (Exception e)
        {
            throw new ZoneUpdateException(
                $"Rating for  Zone with id: {zoneId} has not been updated by feedback's marks");
        }
        
    }

    public async Task UpdateFeedbackAsync(Feedback feedback)
    {
        try
        {
            await _feedbackRepository.UpdateFeedbackAsync(feedback);
        }
        catch (InvalidOperationException)
        {
            throw new FeedbackNotFoundException($"Feedback with id: {feedback.Id} not found ");
        }
        catch (DbUpdateException)
        {
            throw new FeedbackUpdateException($"Feedback with id: {feedback.Id} has not been updated");
        }
    }

    public async Task RemoveFeedbackAsync(Guid feedbackId)
    {
        try
        {
            await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
        }
        catch (InvalidOperationException)
        {
            throw new FeedbackNotFoundException($"Feedback with id: {feedbackId} not found");
        }
        catch (DbUpdateException)
        {
            throw new FeedbackRemoveException($"Feedback with id: {feedbackId} has not been removed");
        }
    }
}
