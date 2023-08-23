namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для создания инвентаря
/// </summary>
public class CreateInventoryDto
{
    public CreateInventoryDto(string name, string description, string yearOfProduction)
    {
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
    }

    /// <summary>
    /// Название инветаря
    /// </summary>
    /// <example>Телевизор</example>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание инветаря
    /// </summary>
    /// <example>Новый инвентарь</example>
    public string Description { get; set; }
    
    /// <summary>
    /// Год выпуска или производства
    /// </summary>
    /// <example>01.12.2023</example>
    public string YearOfProduction { get; set; }
}