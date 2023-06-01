using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.IRepositories;

public interface IStatisticsRepository
{
    Task<List<BookingStatisticsDbModel>> GetBookingStatisticsAsync();
    Task<BookingStatisticsDbModel> GetBookingStatisticsByIdAsync(int id);
    Task<BookingStatisticsDbModel> GetBookingStatisticsByRoomAsync(int roomId);
    Task CreateBookingStatisticsAsync(BookingStatisticsDbModel statistics);
    Task UpdateBookingStatisticsAsync(BookingStatisticsDbModel statistics);
}
