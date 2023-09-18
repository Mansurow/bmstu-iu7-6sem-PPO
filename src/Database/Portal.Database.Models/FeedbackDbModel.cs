using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Database.Models;

/// <summary>
/// Модель базы данных отзыв
/// </summary>
[Table("feedbacks")]
public class FeedbackDbModel
{
    /// <summary>
    /// Идентификатор отзыва
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор зоны
    /// </summary>
    [ForeignKey("Zone")]
    [Column("zone_id")]
    public Guid ZoneId { get; set; }
    
    /// <summary>
    /// Дата и время отзыва
    /// </summary>
    [Column("date")]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Оценка посещение зоны
    /// </summary>
    [Column("mark", TypeName = "numeric")]
    public double Mark { get; set; }
    
    /// <summary>
    /// Сообщение отзыва
    /// </summary>
    [Column("message", TypeName = "text")]
    public string? Message { get; set; }

    /// <summary>
    /// Зона
    /// </summary>
    public ZoneDbModel? Zone { get; set; }
    
    /// <summary>
    /// Пользователь
    /// </summary>
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
