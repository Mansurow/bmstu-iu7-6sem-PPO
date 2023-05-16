using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.IRepositories;

namespace Anticafe.BL.Sevices.FeedbackService;

public class FeedbackService: IFeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;

    public FeedbackService(IFeedbackRepository feedbackRepository,
                           IRoomRepository roomRepository,
                           IUserRepository userRepository)
    {
        _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<List<Feedback>> GetAllFeedbackAsync() 
    {
        return (await _feedbackRepository.GetAllFeedbackAsync())
               .Select(f => FeedbackConverter.ConvertDbModelToAppModel(f)).ToList();
    }

    public async Task<List<Feedback>> GetAllFeedbackByRoomAsync(int roomId) 
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        if (room is null)
        {
           throw new RoomNotFoundException($"Room not found with id: {roomId}");
        }

        var feedbacks = await _feedbackRepository.GetAllFeedbackByRoomAsync(roomId);

        return feedbacks.Select(f => FeedbackConverter.ConvertDbModelToAppModel(f)).ToList();
    }

    public async Task AddFeedbackAsync(int userId, int roomId, int rating, string? msg) 
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        if (room is null)
        {
            throw new RoomNotFoundException($"Room not found with id: {roomId}");
        }  

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new UserNotFoundException($"User not found with id: {userId}");
        }

        var feedback = new Feedback(1, userId, roomId, DateTime.UtcNow, rating, msg);
        await _feedbackRepository.InsertFeedbackAsync(FeedbackConverter.ConvertAppModelToDbModel(feedback));
    }

    public async Task UpdateRoomRatingAsync(int roomId)
    {
        var allRoomFeedback = (await _feedbackRepository.GetAllFeedbackByRoomAsync(roomId))
                              .Select(f => FeedbackConverter.ConvertDbModelToAppModel(f)).ToList();

        var rating = 0;
        foreach (var feedback in allRoomFeedback)
            rating += feedback.Mark;
        
        rating /= allRoomFeedback.Count();

        await _roomRepository.UpdateRoomRaitingAsync(roomId, rating);
    } 

    public async Task UpdateFeedbackAsync(Feedback feedback) 
    {
        var findFeedback = await _feedbackRepository.GetFeedbackAsync(feedback.Id);
        if (findFeedback is null)
        {
            throw new FeedbackNotFoundException($"Feedback not found with id: {feedback.Id}");
        }

        await _feedbackRepository.UpdateFeedbackAsync(FeedbackConverter.ConvertAppModelToDbModel(feedback));
    }

    public async Task DeleteFeedbackAsync(int feedbackId) 
    {
        var feedback = await _feedbackRepository.GetFeedbackAsync(feedbackId);
        if (feedback is null) 
        {
            throw new FeedbackNotFoundException($"Feedback not found with id: {feedbackId}");
        }

        await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
    }


}
