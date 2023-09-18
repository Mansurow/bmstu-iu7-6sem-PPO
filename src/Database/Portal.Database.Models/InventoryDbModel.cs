using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Common.Models;

namespace Portal.Database.Models;

/// <summary>
/// Модель базы данных инвентарь
/// </summary>
[Table("inventories")]
public class InventoryDbModel
{
    /// <summary>
    /// Идентификатор инвентаря
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор зоны
    /// </summary>
    [ForeignKey("Zone")]
    [Column("zone_id")]
    public Guid ZoneId { get; set; }
    
    /// <summary>
    /// Зоны
    /// </summary>
    public ZoneDbModel? Zone { get; set; }
    
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
    /// Дата производтсва или выпуска
    /// </summary>
    [Column("date_production")]
    public DateOnly YearOfProduction { get; set; }

    /// <summary>
    /// Списали ли инвентарь
    /// </summary>
    [Column("is_written_off")]
    public bool IsWrittenOff { get; set; }

    public InventoryDbModel(Guid id, Guid zoneId, string name, string description, DateOnly yearOfProduction, bool isWrittenOff)
    {
        Id = id;
        ZoneId = zoneId;
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
        IsWrittenOff = isWrittenOff;
    }
}
