using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Anticafe.PostgreSQL.Repositories;

public class StatisticsRepository: BaseRepository, IStatisticsRepository
{
    private readonly PgSQLDbContext _context;

    public StatisticsRepository(IDbContextFactory<PgSQLDbContext> contextFactory): base()
    {
        _context = contextFactory.getDbContext();
    }

    public async Task<List<BookingStatisticsDbModel>> GetBookingStatisticsAsync()
    {
        return await _context.BookingStatistics.ToListAsync();
    }

    public async Task<BookingStatisticsDbModel> GetBookingStatisticsByIdAsync(int id)
    {
        return await _context.BookingStatistics.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<BookingStatisticsDbModel> GetBookingStatisticsByRoomAsync(int roomId)
    {
        return await _context.BookingStatistics.FirstOrDefaultAsync(r => r.RoomId == roomId);
    }

    public async Task CreateBookingStatisticsAsync(BookingStatisticsDbModel statistics)
    {
        try
        {
            _context.BookingStatistics.Add(statistics);
            await _context.SaveChangesAsync();
        } catch
        {
            throw new Exception($"Statistics not create for room with id: {statistics.RoomId}");
        }
    }

    public async Task UpdateBookingStatisticsAsync(BookingStatisticsDbModel statistics)
    {
        try
        {
            var find = await _context.BookingStatistics.FirstOrDefaultAsync(u => u.Id == statistics.Id);
            if (find is not null)
            {
                find.TotalCount = statistics.TotalCount;
                find.AvgDuration = statistics.AvgDuration;
                find.MaxDuration = statistics.MaxDuration;
            }

            await _context.SaveChangesAsync();
        } catch
        {
            throw new Exception($"Statistics not update for room with id: {statistics.RoomId}");
        }
    }
}
