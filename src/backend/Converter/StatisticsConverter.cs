using Anticafe.BL.Models;
using Anticafe.Common.Models.DTO;
using Anticafe.DataAccess.DBModels;

namespace Anticafe.Converter;

public class StatisticsConverter
{
    public static BookingStatistics ConvertDbModelToAppModel(BookingStatisticsDbModel statistics)
    {
        return new BookingStatistics(id: statistics.Id,
                                     roomId: statistics.RoomId,
                                     totalCount: statistics.TotalCount,
                                     avgDuration: statistics.AvgDuration,
                                     maxDuration: statistics.MaxDuration);
    }

    public static BookingStatisticsDbModel ConvertAppModelToDbModel(BookingStatistics statistics)
    {
        return new BookingStatisticsDbModel(id: statistics.Id,
                                     roomId: statistics.RoomId,
                                     totalCount: statistics.TotalCount,
                                     avgDuration: statistics.AvgDuration,
                                     maxDuration: statistics.MaxDuration);
    }

    public static BookingStatisticsDto ConvertAppModelToDto(BookingStatistics statistics)
    {
        return new BookingStatisticsDto(
                                     roomId: statistics.RoomId,
                                     totalCount: statistics.TotalCount,
                                     avgDuration: statistics.AvgDuration,
                                     maxDuration: statistics.MaxDuration);
    }
}
