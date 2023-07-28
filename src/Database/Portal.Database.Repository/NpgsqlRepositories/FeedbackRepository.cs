using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Database.Context;
using Portal.Database.Models;
using Portal.Database.Repositories.Interfaces;

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
            .Select(f => FeedbackConverter.ConvertDbModelToAppModel(f)!)
            .ToListAsync();
    }

    public Task<List<Feedback>> GetAllFeedbackUserAsync(Guid userId)
    {
        return _context.Feedbacks
            .Where(f => f.UserId == userId)
            .Select(f => FeedbackConverter.ConvertDbModelToAppModel(f)!)
            .ToListAsync();
    }

    public Task<List<Feedback>> GetAllFeedbackAsync()
    {
        return _context.Feedbacks
            .Select(f => FeedbackConverter.ConvertDbModelToAppModel(f)!)
            .ToListAsync();
    }

    public async Task<Feedback> GetFeedbackAsync(Guid feedbackId)
    {
        var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId);

        return FeedbackConverter.ConvertDbModelToAppModel(feedback);
    }

    public async Task InsertFeedbackAsync(Feedback feedback)
    {
        var feedbackDb = FeedbackConverter.ConvertAppModelToDbModel(feedback);
        
        await _context.Feedbacks.AddAsync(feedbackDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFeedbackAsync(Feedback feedback)
    {
        var feedbackDb = FeedbackConverter.ConvertAppModelToDbModel(feedback);

        _context.Feedbacks.Update(feedbackDb);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFeedbackAsync(Guid feedbackId)
    {
        var feedbackDb = await _context.Feedbacks.FirstAsync(f => f.Id == feedbackId);

        _context.Feedbacks.Remove(feedbackDb);
        await _context.SaveChangesAsync();
    }
}