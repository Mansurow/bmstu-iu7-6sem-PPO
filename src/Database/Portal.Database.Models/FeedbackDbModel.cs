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
    [ForeignKey("Zone")]
    [Column("zone_id")]
    public Guid ZoneId { get; set; }
    [Column("date", TypeName = "varchar(64)")]
    public DateTime Date { get; set; }
    [Column("mark")]
    public double Mark { get; set; }
    [Column("message", TypeName = "text")]
    public string? Message { get; set; }

    public ZoneDbModel? Zone { get; set; }
    public UserDbModel? User { get; set; }

    public FeedbackDbModel(Guid id, Guid userId, Guid zoneId, DateTime date, double mark, string? message)
    {
        Id = id;
        UserId = userId;
        ZoneId = zoneId;
        Date = date;
        Mark = mark;
        Message = message;
    }
}
