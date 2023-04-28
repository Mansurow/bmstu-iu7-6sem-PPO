using Anticafe.Common.Enums;

namespace Anticafe.BL.Models;

public class Booking
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int UserId { get; set; }
    public int AmountPeople { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; } 

    public Booking(int id, int roomId, int userId, int amountPeople, DateTime startTime, DateTime endTime, BookingStatus status)
    {
        Id = id;
        RoomId = roomId;
        UserId = userId;
        AmountPeople = amountPeople;
        Status = status;
        StartTime = startTime;
        EndTime = endTime;
    }

    public void ChangeStatus(BookingStatus status)
    {
        Status = status;
    }

    public bool IsBookingExpired()
    {
        return EndTime >= DateTime.UtcNow;
    }
}
