﻿using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Dto.Feedback;

/// <summary>
/// Отзыв.
/// </summary>
public class Feedback
{
    /// <summary>
    /// Идентификатор отзыва.
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя, который оставил отзыв.
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор комнаты/зала/зоны.
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid ZoneId { get; set; }
    
    /// <summary>
    /// Время и дата отзыва.
    /// </summary>
    /// <example>17-12-2023 12:00</example>
    [Required]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Оценка с отзывом по 5-бальной шкале.
    /// </summary>
    /// <example>5.0</example>
    [Required]
    public double Mark { get; set; }
    
    /// <summary>
    /// Описание отзыва. 
    /// </summary>
    /// <example>Все было круто!</example>
    public string? Message { get; set; }
    
    public Feedback(Guid id, Guid userId, Guid zoneId, DateTime date, double mark, string? message)
    {
        Id = id;
        UserId = userId;
        ZoneId = zoneId;
        Date = date;
        Mark = mark;
        Message = message;
    }
}