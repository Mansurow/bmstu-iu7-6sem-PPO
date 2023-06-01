using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.IRepositories;

public interface IStatisticsRepository
{
    Task<List<BookingStatisticsDbModel>> GetBookingStatisticsAsync();
    Task<BookingStatisticsDbModel> GetBookingStatisticsByRoomAsync(int roomId);
    Task CreateBookingStatisticsAsync(int roomId, double avgDuration, double maxDuration, );
    Task UpdateBookingStatisticsAsync(BookingStatisticsDbModel bookingStatistics);
}
