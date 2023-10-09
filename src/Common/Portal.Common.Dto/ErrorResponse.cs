using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Dto;

/// <summary>
/// Модель ответа сервера в случае ошибки.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Название класса исключения.
    /// </summary>
    [Required]
    public string? ErrorType { get; set; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    [Required]
    public string Message { get; set; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    public ErrorResponse(string message, string? errorType = null)
    {
        ErrorType = errorType;
        Message = message;
    }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    public ErrorResponse(Exception exception)
    {
        ErrorType = exception.GetType().ToString();
        Message = exception.Message;
    }

}