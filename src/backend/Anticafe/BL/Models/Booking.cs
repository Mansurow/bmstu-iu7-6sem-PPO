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

    // Может  EndTime заменить на количество часов аренды, а не на когда заканчивается ...

    public Booking(int id, int roomId, int userId, DateTime startTime, DateTime dateTime, BookingStatus status)
    {
        Id = id;
        RoomId = roomId;
        UserId = userId;
        Status = status;
        StartTime = startTime;
        EndTime = dateTime;
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
