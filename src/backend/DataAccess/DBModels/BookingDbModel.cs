using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Anticafe.Common.Enums;

namespace Anticafe.DataAccess.DBModels;
public class BookingDbModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("Room")]
    [Column("room_id")]
    public int RoomId { get; set; }
    public RoomDbModel? Room { get; set; }

    [ForeignKey("User")]
    [Column("user_id")]
    public int UserId { get; set; }
    public UserDbModel? User { get; set; }

    [Column("amount_of_people", TypeName = "integer")]
    public int AmountPeople { get; set; }
    [Column("status", TypeName = "integer")]
    public BookingStatus Status { get; set; }
    [Column("start_time", TypeName = "varchar(64)")]
    public DateTime StartTime { get; set; }
    [Column("end_time", TypeName = "varchar(64)")]
    public DateTime EndTime { get; set; }

    public BookingDbModel(int id, int roomId, int userId, int amountPeople, DateTime startTime, DateTime endTime, BookingStatus status)
    {
        Id = id;
        RoomId = roomId;
        UserId = userId;
        AmountPeople = amountPeople;
        Status = status;
        StartTime = startTime;
        EndTime = endTime;
    }
}