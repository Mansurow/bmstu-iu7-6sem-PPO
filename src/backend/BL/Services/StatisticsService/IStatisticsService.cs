using Anticafe.BL.Models;

namespace Anticafe.BL.Services.StatisticsService;

public interface IStatisticsService
{
    Task<List<BookingStatistics>> GetBookingStatisticsAsync();
    Task<BookingStatistics> GetBookingStatisticsByRoomAsync(int roomId);
    Task CalculateBookingStatisticsForRoomAsync(int roomId);
}
