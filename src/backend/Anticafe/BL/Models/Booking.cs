using Anticafe.BL.Enums;

namespace Anticafe.BL.Models;

public class Booking
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int UserId { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    // public TimeSpan TimeBooking { get; set; }

    // подумать про время !!!

    Booking(int id, int roomId, int userId, BookingStatus status, DateTime startTime, DateTime dateTime)
    {
        Id = id;
        RoomId = roomId;
        UserId = userId;
        Status = status;
        StartTime = startTime;
        EndTime = dateTime;
    }
}
