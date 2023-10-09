using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Common.Core;
using Portal.Database.Core.Repositories;
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
    private readonly ILogger<FeedbackService> _logger;

    public FeedbackService(IFeedbackRepository feedbackRepository,
                           IZoneRepository zoneRepository,
                           IUserRepository userRepository,
                           ILogger<FeedbackService> logger)
    {
        _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
        _zoneRepository = zoneRepository ?? throw new ArgumentNullException(nameof(zoneRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            _logger.LogError(e, "Zone with id: {ZoneId} not found", zoneId);
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
        }
    }

    public async Task<Guid> AddFeedbackAsync(Guid zoneId, Guid userId, double mark, string description)
    {
        try
        {
            var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
            
            try
            {
                await _userRepository.GetUserByIdAsync(userId);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "User with id: {UserId} not found", userId);
                throw new UserNotFoundException($"User with id: {userId} not found");
            }

            var feedback = new Feedback(Guid.NewGuid(), userId, zone.Id,
                DateTime.UtcNow, mark, description);
            await _feedbackRepository.InsertFeedbackAsync(feedback);

            return feedback.Id;
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Zone with id: {ZoneId} not found", zoneId);
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while creating feedback by user: {UserId} for zone: {ZoneId}", userId, zoneId);
            throw new FeedbackCreateException($"Feedback has not been created by user: {userId} or zone: {zoneId}");
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
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Rating for zone with id: {ZoneId} has not been updated by feedback's marks", zoneId);
            throw new ZoneUpdateException(
                $"Rating for zone with id: {zoneId} has not been updated by feedback's marks");
        }
        
    }

    public async Task UpdateFeedbackAsync(Feedback feedback)
    {
        try
        {
            await _feedbackRepository.UpdateFeedbackAsync(feedback);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Feedback with id: {FeedbackId} not found", feedback.Id);
            throw new FeedbackNotFoundException($"Feedback with id: {feedback.Id} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while updating feedback: {FeedbackId}", feedback.Id);
            throw new FeedbackUpdateException($"Feedback with id: {feedback.Id} has not been updated");
        }
    }

    public async Task RemoveFeedbackAsync(Guid feedbackId)
    {
        try
        {
            await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Feedback with id: {FeedbackId} not found", feedbackId);
            throw new FeedbackNotFoundException($"Feedback with id: {feedbackId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while removing feedback: {FeedbackId}", feedbackId);
            throw new FeedbackRemoveException($"Feedback with id: {feedbackId} has not been removed");
        }
    }
}
