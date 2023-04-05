using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.FeedbackService
{
    public class FeedbackService: IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IRoomRepository _roomRepository;

        FeedbackService(IFeedbackRepository feedbackRepository,
                        IRoomRepository roomRepository)
        {
            _feedbackRepository = feedbackRepository;
            _roomRepository = roomRepository;
        }

        public async Task<List<Feedback>> GetAllFeedbackByRoomAsync(int roomId) 
        {
            var room = await _roomRepository.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                new Exception();
            }

            var feedbacks = await _feedbackRepository.GetAllFeedbackByRoomAsync(roomId);

            return feedbacks;
        }

        public async Task AddFeedbackAsync(Feedback feedback) 
        {
            var room = await _roomRepository.GetRoomByIdAsync(feedback.RoomId);
            if (room == null)
            {
                new Exception();
            }

            await _feedbackRepository.InsertFeedbackAsync(feedback);
        }

        public async Task UpdateFeedbackAsync(Feedback feedback) 
        {
            var room = await _roomRepository.GetRoomByIdAsync(feedback.RoomId);
            if (room == null)
            {
                new Exception();
            }

            await _feedbackRepository.UpdateFeedbackAsync(feedback);
        }

        public async Task DeleteFeedbackAsync(int feedbackId) 
        {
            var feedback = await _feedbackRepository.GetFeedbackAsync(feedbackId);
            if (feedback == null) 
            {
                new Exception();
            }

            await _feedbackRepository.DeleteFeedbackByFeedbackIdAsync(feedbackId);
        }
    }
}
