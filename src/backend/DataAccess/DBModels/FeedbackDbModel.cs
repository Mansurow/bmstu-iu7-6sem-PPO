using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Anticafe.DataAccess.DBModels;
public class FeedbackDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }
    [ForeignKey("User")]
    [Column("user_id")]
    public int UserId { get; set; }
    [ForeignKey("Room")]
    [Column("room_id")]
    public int RoomId { get; set; }
    [Column("date", TypeName = "varchar(64)")]
    public DateTime Date { get; set; }
    [Column("mark")]
    public int Mark { get; set; }
    [Column("message", TypeName = "text")]
    public string? Message { get; set; }

    [BsonIgnore]
    public RoomDbModel? Room { get; set; }
    [BsonIgnore]
    public UserDbModel? User { get; set; }

    public FeedbackDbModel(int id, int userId, int roomId, DateTime date, int mark, string? message)
    {
        Id = id;
        UserId = userId;
        RoomId = roomId;
        Date = date;
        Mark = mark;
        Message = message;
    }
}
