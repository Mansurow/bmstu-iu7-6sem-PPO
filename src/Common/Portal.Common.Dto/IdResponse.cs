namespace Portal.Common.Dto;

/// <summary>
/// Модель ответа с идентификатором
/// </summary>
public class IdResponse
{
    /// <summary>
    /// Идентификатор объекта
    /// </summary>
    public Guid Id { get; set; }

    public IdResponse(Guid id)
    {
        Id = id;
    }
}