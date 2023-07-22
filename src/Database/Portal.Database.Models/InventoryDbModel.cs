using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Database.Models;

/// <summary>
/// Модель базы данных инвентарь
/// </summary>
public class InventoryDbModel
{
    /// <summary>
    /// Идентификатор инвентаря
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Название инвентаря
    /// </summary>
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    
    /// <summary>
    /// Описание инвентаря
    /// </summary>
    [Column ("description", TypeName = "text")]
    public string Description { get; set; }
    
    /// <summary>
    /// Год производтсва или выпуска
    /// </summary>
    [Column("year_of_production")]
    public DateOnly YearOfProduction { get; set; }

    public InventoryDbModel(Guid id, string name, string description, DateOnly yearOfProduction)
    {
        Id = id;
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
    }
}
