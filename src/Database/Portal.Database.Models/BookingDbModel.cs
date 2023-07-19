using Portal.Common.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Database.Models;
public class BookingDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [ForeignKey("Room")]
    [Column("room_id")]
    public Guid RoomId { get; set; }
    public ZoneDbModel? Room { get; set; }

    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    public UserDbModel? User { get; set; }

    [Column("amount_of_people", TypeName = "integer")]
    public int AmountPeople { get; set; }
    [Column("status", TypeName = "integer")]
    public BookingStatus Status { get; set; }
    [Column ("date")]
    public DateOnly Date { get; set; }
    [Column("start_time")]
    public TimeOnly StartTime { get; set; }
    [Column("end_time")]
    public TimeOnly EndTime { get; set; }

    public BookingDbModel(Guid id, Guid roomId, Guid userId, int amountPeople, DateOnly date, TimeOnly startTime, TimeOnly endTime, BookingStatus status)
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
}