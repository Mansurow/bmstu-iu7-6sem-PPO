namespace Portal.Common.Models;

/// <summary>
/// Инвентарь зоны
/// </summary>
public class Inventory
{
    /// <summary>
    /// Идентификатор инветаря
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор зоны
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid ZoneId { get; set; }
    
    /// <summary>
    /// Название инветаря
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание инветаря
    /// </summary>
    /// <example>Новый инвентарь</example>
    public string Description { get; set; }
    
    /// <summary>
    /// Год выпуска или производства
    /// </summary>
    /// <example>10.12.2023</example>
    public DateOnly YearOfProduction { get; set; }

    /// <summary>
    /// Списали ли инвентарь
    /// </summary>
    public bool IsWrittenOff { get; set; }

    public Inventory(Guid id, Guid zoneId, string name, string description, DateOnly yearOfProduction, bool isWrittenOff = false)
    {
        Id = id;
        ZoneId = zoneId;
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
        IsWrittenOff = isWrittenOff;
    }
}
