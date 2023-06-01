using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Anticafe.PostgreSQL.Repositories;

public class StatisticsRepository: BaseRepository, IStatisticsRepository
{
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly PgSQLDbContext _context;

    public StatisticsRepository(IDbContextFactory<PgSQLDbContext> contextFactory): base()
    {
        _context = contextFactory.getDbContext();
    }

    public async Task<List<BookingStatisticsDbModel>> GetBookingStatisticsAsync()
    {
        return await _context.BookingStatistics.ToListAsync();
    }

    public async Task<BookingStatisticsDbModel> GetBookingStatisticsByRoomAsync(int roomId)
    {
        return await _context.BookingStatistics.FirstOrDefaultAsync(r => r.RoomId == roomId);
    }
}
