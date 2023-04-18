using Anticafe.BL.Exceptions;
using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;

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
        return await _feedbackRepository.GetAllFeedbackAsync();
    }

    public async Task<List<Feedback>> GetAllFeedbackByRoomAsync(int roomId) 
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        if (room is null)
        {
           throw new RoomNotFoundException($"Room not found with id: {roomId}");
        }

        var feedbacks = await _feedbackRepository.GetAllFeedbackByRoomAsync(roomId);

        return feedbacks;
    }

    public async Task AddFeedbackAsync(Feedback feedback) 
    {
        var room = await _roomRepository.GetRoomByIdAsync(feedback.RoomId);
        if (room is null)
        {
            throw new RoomNotFoundException($"Room not found with id: {feedback.RoomId}");
        }  

        var user = await _userRepository.GetUserByIdAsync(feedback.UserId);
        if (user is null)
        {
            throw new UserNotFoundException($"User not found with id: {feedback.UserId}");
        }

        await _feedbackRepository.InsertFeedbackAsync(feedback);
    }

    public async Task UpdateRoomRatingAsync(int roomId)
    {
        var allRoomFeedback = await _feedbackRepository.GetAllFeedbackByRoomAsync(roomId);

        var rating = 0;
        foreach (var feedback in allRoomFeedback)
            rating += feedback.Mark;
        
        rating /= allRoomFeedback.Count();

        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        room.ChangeRating(rating);

        await _roomRepository.UpdateRoomAsync(room);
    } 

    public async Task UpdateFeedbackAsync(Feedback feedback) 
    {
        var findFeedback = await _feedbackRepository.GetFeedbackAsync(feedback.RoomId);
        if (findFeedback is null)
        {
            throw new FeedbackNotFoundException($"Feedback not found with id: {feedback.RoomId}");
        }

        await _feedbackRepository.UpdateFeedbackAsync(feedback);
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
