using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Anticafe.DataAccess.Repositories;

public class FeedbackRepository: BaseRepository, IFeedbackRepository
{
    private readonly AppDbContext _context;

    public FeedbackRepository(IDbContextFactory contextFactory) : base()
    {
        _context = contextFactory.getDbContext();
    }

    public async Task<List<FeedbackDbModel>> GetAllFeedbackByRoomAsync(int roomId) 
    {
        return await _context.Feedbacks.Where(f => f.RoomId == roomId).ToListAsync();
    }

    public async Task<List<FeedbackDbModel>> GetAllFeedbackUserAsync(int userId) 
    {
        return await _context.Feedbacks.Where(f => f.UserId == userId).ToListAsync();
    }

    public async Task<List<FeedbackDbModel>> GetAllFeedbackAsync()
    {
        return await _context.Feedbacks.ToListAsync();
    }

    public async Task<FeedbackDbModel> GetFeedbackAsync(int feedbackId) 
    {
        var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId);
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
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        } catch
        {
            throw new FeedbackCreateException("Feedback not create");
        }
    }

    public async Task UpdateFeedbackAsync(FeedbackDbModel feedback) 
    {
        try
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
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
            var feedback = await GetFeedbackAsync(feedbackId);
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
        }
        catch
        {
            throw new FeedbackDeleteException("Feedback not delete");
        }
    }
}
