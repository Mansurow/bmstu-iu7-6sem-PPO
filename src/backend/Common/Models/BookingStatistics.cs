using Microsoft.AspNetCore.Routing.Constraints;

namespace Anticafe.BL.Models;

public class BookingStatistics
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public double AvgDuration { get; set; }
    public double MaxDuration { get; set; }
    public int TotalCount { get; set; }

    public BookingStatistics(int id, int roomId, double avgDuration, double maxDuration, int totalCount)
    {
        Id = id;
        RoomId = roomId;
        AvgDuration = avgDuration;
        MaxDuration = maxDuration;
        TotalCount = totalCount;
    }
}
