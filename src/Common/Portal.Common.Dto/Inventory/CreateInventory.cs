using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Dto.Inventory;

/// <summary>
/// Модель для создания инвентаря.
/// </summary>
public class CreateInventory
{
    /// <summary>
    /// Название инветаря.
    /// </summary>
    /// <example>Телевизор</example>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Описание инветаря.
    /// </summary>
    /// <example>Новый инвентарь</example>
    [Required]
    public string Description { get; set; }
    
    /// <summary>
    /// Год выпуска или производства.
    /// </summary>
    /// <example>01.12.2023</example>
    [Required]
    public string YearOfProduction { get; set; }
    
    public CreateInventory(string name, string description, string yearOfProduction)
    {
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
    }
}