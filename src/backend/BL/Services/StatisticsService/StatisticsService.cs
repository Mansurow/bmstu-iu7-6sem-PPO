using System.Runtime.InteropServices;
using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.Converter;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;

namespace Anticafe.BL.Services.StatisticsService;

public class StatisticsService: IStatisticsService
{
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;

    public StatisticsService(IStatisticsRepository statisticsRepository, IRoomRepository roomRepository, IBookingRepository bookingRepository)
    {
        _statisticsRepository = statisticsRepository;
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<List<BookingStatistics>> GetBookingStatisticsAsync()
    {
        return (await _statisticsRepository.GetBookingStatisticsAsync()).Select(StatisticsConverter.ConvertDbModelToAppModel).ToList();
    }

    public async Task<BookingStatistics> GetBookingStatisticsByRoomAsync(int roomId)
    {
        var room = _roomRepository.GetRoomByIdAsync(roomId);
        if (room is null)
        {
            throw new RoomNotFoundException($"Room Not Found");
        }

        var statistics = await _statisticsRepository.GetBookingStatisticsByRoomAsync(roomId);
        if (statistics is null)
        {
            throw new Exception($"Statistics not found for room: {roomId}");
        }

        return StatisticsConverter.ConvertDbModelToAppModel(statistics);
    }

    public async Task CalculateBookingStatisticsForRoomAsync(int roomId)
    {
        var room = _roomRepository.GetRoomByIdAsync(roomId);
        if (room is null)
        {
            throw new RoomNotFoundException($"Room Not Found");
        }

        var roomBookings = await _bookingRepository.GetBookingByRoomAsync(roomId);
        if (roomBookings.Count != 0)
        {
            var timeDuration = roomBookings.Select(b => b.EndTime - b.StartTime);

            var allst = await GetBookingStatisticsAsync();

            var statistics = new BookingStatisticsDbModel(allst.Count + 1, roomId, 0, 0, roomBookings.Count);

            if (allst.First(b => b.Id == roomId) != null)
            {
                await _statisticsRepository.UpdateBookingStatisticsAsync(statistics);
            } else
            {
                await _statisticsRepository.CreateBookingStatisticsAsync(statistics);
            }
        } 
    }
}
