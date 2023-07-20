using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.FeedbackService.Exceptions;
using Portal.Services.UserService.Exceptions;
using Portal.Services.ZoneService.Exceptions;

namespace Portal.Services.FeedbackService;

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
        var room = await _zoneRepository.GetZoneByIdAsync(zoneId);
        if (room is null)
        {
            throw new ZoneNotFoundException($"Zone not found with id: {zoneId}");
        }

        var feedbacks = await _feedbackRepository.GetAllFeedbackByZoneAsync(zoneId);

        return feedbacks;
    }

    public async Task AddFeedbackAsync(Guid zoneId, Guid userId, double mark, string description)
    {
        var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
        if (zone is null)
        {
            throw new ZoneNotFoundException($"Zone not found with id: {zoneId}");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new UserNotFoundException($"User not found with id: {userId}");
        }

        await _feedbackRepository.InsertFeedbackAsync(new Feedback(Guid.NewGuid(), userId, zoneId, 
            DateTime.UtcNow, mark, description));
    }

    public async Task UpdateZoneRatingAsync(Guid zoneId)
    {
        var allZoneFeedback = await _feedbackRepository.GetAllFeedbackByZoneAsync(zoneId);

        var rating = allZoneFeedback.Sum(feedback => feedback.Mark);
        if (allZoneFeedback.Count > 0)
            rating /= allZoneFeedback.Count;

        await _zoneRepository.UpdateZoneRatingAsync(zoneId, rating);
    }

    public async Task UpdateFeedbackAsync(Feedback feedback)
    {
        var findFeedback = await _feedbackRepository.GetFeedbackAsync(feedback.Id);
        if (findFeedback is null)
        {
            throw new FeedbackNotFoundException($"Feedback not found with id: {feedback.Id}");
        }

        await _feedbackRepository.UpdateFeedbackAsync(feedback);
    }

    public async Task RemoveFeedbackAsync(Guid feedbackId)
    {
        var feedback = await _feedbackRepository.GetFeedbackAsync(feedbackId);
        if (feedback is null)
        {
            throw new FeedbackNotFoundException($"Feedback not found with id: {feedbackId}");
        }

        await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
    }


}
