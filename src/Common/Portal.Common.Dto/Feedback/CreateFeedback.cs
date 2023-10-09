using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Dto.Feedback;

/// <summary>
/// Модель для создания отзыва
/// </summary>
public class CreateFeedback
{
    public CreateFeedback(Guid userId, Guid zoneId, double mark, string message)
    {
        UserId = userId;
        ZoneId = zoneId;
        Mark = mark;
        Message = message;
    }

    /// <summary>
    /// Идентификатор пользователя, который оставил отзыв
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор комнаты/зала/зоны
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid ZoneId { get; set; }

    /// <summary>
    /// Оценка с отзывом по 5-бальной шкале
    /// </summary>
    /// <example>5.0</example>
    [Required]
    public double Mark { get; set; }
    
    /// <summary>
    /// Описание отзыва 
    /// </summary>
    /// <example>Все было круто!</example>
    [Required]
    public string Message { get; set; }
}