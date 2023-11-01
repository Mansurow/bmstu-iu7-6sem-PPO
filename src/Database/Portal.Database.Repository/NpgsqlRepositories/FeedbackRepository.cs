using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Core;
using Portal.Database.Context;
using Portal.Database.Core.Repositories;

namespace Portal.Database.Repositories.NpgsqlRepositories;

public class FeedbackRepository: BaseRepository, IFeedbackRepository
{
    private readonly PortalDbContext _context;

    public FeedbackRepository(PortalDbContext context)
    {
        _context = context;
    }
    
    public Task<List<Feedback>> GetAllFeedbackByZoneAsync(Guid zoneId)
    {
        return _context.Feedbacks
            .Where(f => f.ZoneId == zoneId)
            .OrderBy(f => f.Date)
            .Select(f => FeedbackConverter.ConvertDBToCoreModel(f))
            .ToListAsync();
    }

    public Task<List<Feedback>> GetAllFeedbackUserAsync(Guid userId)
    {
        return _context.Feedbacks
            .Where(f => f.UserId == userId)
            .OrderBy(f => f.Date)
            .Select(f => FeedbackConverter.ConvertDBToCoreModel(f))
            .ToListAsync();
    }

    public Task<List<Feedback>> GetAllFeedbackAsync()
    {
        return _context.Feedbacks
            .OrderBy(f => f.Date)
            .Select(f => FeedbackConverter.ConvertDBToCoreModel(f))
            .ToListAsync();
    }

    public async Task<Feedback> GetFeedbackAsync(Guid feedbackId)
    {
        var feedback = await _context.Feedbacks.FirstAsync(f => f.Id == feedbackId);

        return FeedbackConverter.ConvertDBToCoreModel(feedback);
    }

    public async Task InsertFeedbackAsync(Feedback feedback)
    {
        var feedbackDb = FeedbackConverter.ConvertCoreToDBModel(feedback);
        
        await _context.Feedbacks.AddAsync(feedbackDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFeedbackAsync(Feedback feedback)
    {
        var feedbackDb = await _context.Feedbacks.FirstAsync(f => f.Id == feedback.Id);

        feedbackDb.Mark = feedback.Mark;
        feedbackDb.Message = feedback.Message;
        // feedbackDb.ChangeTime = feedback.ChangeTime; // TODO: добавить время изменения
        // Возможно флаг изменения IsChanged    
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFeedbackAsync(Guid feedbackId)
    {
        var feedbackDb = await _context.Feedbacks.FirstAsync(f => f.Id == feedbackId);

        _context.Feedbacks.Remove(feedbackDb);
        await _context.SaveChangesAsync();
    }
}