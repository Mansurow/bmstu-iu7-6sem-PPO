using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.FeedbackService.Exceptions;

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

    public async Task<List<Feedback>> GetAllFeedbackByRoomAsync(Guid zoneId)
    {
        var room = await _zoneRepository.GetZoneByIdAsync(zoneId);
        if (room is null)
        {
            throw new Exception($"Room not found with id: {zoneId}");
        }

        var feedbacks = await _feedbackRepository.GetAllFeedbackByZoneAsync(zoneId);

        return feedbacks;
    }

    public async Task AddFeedbackAsync(Feedback feedback)
    {
        var room = await _zoneRepository.GetZoneByIdAsync(feedback.RoomId);
        if (room is null)
        {
            throw new Exception($"Room not found with id: {feedback.RoomId}");
        }

        var user = await _userRepository.GetUserByIdAsync(feedback.UserId);
        if (user is null)
        {
            throw new Exception($"User not found with id: {feedback.UserId}");
        }

        await _feedbackRepository.InsertFeedbackAsync(feedback);
    }

    public async Task UpdateZoneRatingAsync(Guid zoneId)
    {
        var allZoneFeedback = await _feedbackRepository.GetAllFeedbackByZoneAsync(zoneId);

        var rating = 0.0;
        foreach (var feedback in allZoneFeedback)
            rating += feedback.Mark;

        rating /= allZoneFeedback.Count();

        await _zoneRepository.UpdateZoneRaitingAsync(zoneId, rating);
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

    public async Task DeleteFeedbackAsync(Guid feedbackId)
    {
        var feedback = await _feedbackRepository.GetFeedbackAsync(feedbackId);
        if (feedback is null)
        {
            throw new FeedbackNotFoundException($"Feedback not found with id: {feedbackId}");
        }

        await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
    }


}
