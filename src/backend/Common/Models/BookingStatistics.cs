using Microsoft.AspNetCore.Routing.Constraints;

namespace Anticafe.BL.Models;

public class BookingStatistics
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public double AvgDuration { get; set; }
    public double MaxDuration { get; set; }
    public int TotalCount { get; set; }
    // public TimeOnly PopularBookingTime { get; set; }
}
