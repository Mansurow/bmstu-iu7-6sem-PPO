using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Anticafe.MongoDB.Repositories;

public class FeedbackRepository: BaseRepository, IFeedbackRepository
{
    private readonly IMongoCollection<FeedbackDbModel> _feedbackCollection;

    public FeedbackRepository(IDbCollectionFactory collections) : base()
    {
        _feedbackCollection = collections.GetFeedbackCollection();
    }

    public async Task<List<FeedbackDbModel>> GetAllFeedbackByRoomAsync(int roomId) 
    {
        return await _feedbackCollection.Find(f => f.RoomId == roomId).ToListAsync();
    }

    public async Task<List<FeedbackDbModel>> GetAllFeedbackUserAsync(int userId) 
    {
        return await _feedbackCollection.Find(f => f.UserId == userId).ToListAsync();
    }

    public async Task<List<FeedbackDbModel>> GetAllFeedbackAsync()
    {
        return await _feedbackCollection.Find(_ => true).ToListAsync();
    }

    public async Task<FeedbackDbModel> GetFeedbackAsync(int feedbackId) 
    {
        var feedback = await _feedbackCollection.Find(f => f.Id == feedbackId).FirstOrDefaultAsync();
        if (feedback is null) 
        {
            throw new FeedbackNotFoundException("Feedback not found");
        }

        return feedback;
    }

    public async Task InsertFeedbackAsync(FeedbackDbModel feedback) 
    {
        try
        {
            await _feedbackCollection.InsertOneAsync(feedback);
        } catch
        {
            throw new FeedbackCreateException("Feedback not create");
        }
    }

    public async Task UpdateFeedbackAsync(FeedbackDbModel feedback) 
    {
        try
        {
            var filter = Builders<FeedbackDbModel>.Filter.Eq(u => u.Id, feedback.Id);
            var update = Builders<FeedbackDbModel>.Update.Set(u => u.Mark, feedback.Mark)
                                                         .Set(u => u.Message, feedback.Message);
            await _feedbackCollection.UpdateOneAsync(filter, update);
        }
        catch
        {
            throw new FeedbackUpdateException("Feedback not update");
        }
    }

    public async Task DeleteFeedbackAsync(int feedbackId) 
    {
        try
        {
            var filter = Builders<FeedbackDbModel>.Filter.Lt(u => u.Id, feedbackId);
            await _feedbackCollection.DeleteOneAsync(filter);
        }
        catch
        {
            throw new FeedbackDeleteException("Feedback not delete");
        }
    }
}
