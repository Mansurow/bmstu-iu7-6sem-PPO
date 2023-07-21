namespace Portal.Common.Models;

/// <summary>
/// Инвентарь зоны
/// </summary>
public class Inventory
{
    /// <summary>
    /// Идентификатор инветаря
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название инветаря
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание инветаря
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Год выпуска или производства
    /// </summary>
    public DateOnly YearOfProduction { get; set; }

    public Inventory(Guid id, string name, string description, DateOnly yearOfProduction)
    {
        Id = id;
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
    }
}
