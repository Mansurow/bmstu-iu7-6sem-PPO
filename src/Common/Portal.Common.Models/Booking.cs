using Portal.Common.Models.Enums;

namespace Portal.Common.Models;

public class Booking
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid UserId { get; set; }
    public int AmountPeople { get; set; }
    public BookingStatus Status { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public Booking(Guid id, Guid roomId, Guid userId, int amountPeople, BookingStatus status, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        Id = id;
        RoomId = roomId;
        UserId = userId;
        AmountPeople = amountPeople;
        Status = status;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }

    public void ChangeStatus(BookingStatus status)
    {
        Status = status;
    }

    public bool IsBookingExpired()
    {
        return EndTime <= TimeOnly.FromDateTime(DateTime.UtcNow);
    }
}
