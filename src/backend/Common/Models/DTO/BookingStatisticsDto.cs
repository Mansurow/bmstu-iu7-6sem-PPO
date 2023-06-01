namespace Anticafe.Common.Models.DTO;

public class BookingStatisticsDto
{
    public int RoomId { get; set; }
    public double AvgDuration { get; set; }
    public double MaxDuration { get; set; }
    public int TotalCount { get; set; }

    public BookingStatisticsDto(int roomId, double avgDuration, double maxDuration, int totalCount)
    {
        RoomId = roomId;
        AvgDuration = avgDuration;
        MaxDuration = maxDuration;
        TotalCount = totalCount;
    }
}
