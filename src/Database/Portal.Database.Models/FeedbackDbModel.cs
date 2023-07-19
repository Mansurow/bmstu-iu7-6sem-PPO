using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Database.Models;
public class FeedbackDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    [ForeignKey("Room")]
    [Column("room_id")]
    public Guid RoomId { get; set; }
    [Column("date", TypeName = "varchar(64)")]
    public DateTime Date { get; set; }
    [Column("mark")]
    public double Mark { get; set; }
    [Column("message", TypeName = "text")]
    public string? Message { get; set; }

    public ZoneDbModel? Room { get; set; }
    public UserDbModel? User { get; set; }

    public FeedbackDbModel(Guid id, Guid userId, Guid roomId, DateTime date, double mark, string? message)
    {
        Id = id;
        UserId = userId;
        RoomId = roomId;
        Date = date;
        Mark = mark;
        Message = message;
    }
}
