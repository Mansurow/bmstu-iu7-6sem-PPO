using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Common.Models.Enums;

namespace Portal.Database.Models;

/// <summary>
/// Модель базы данных пакет
/// </summary>
[Table("packages")]
public class PackageDbModel
{
    /// <summary>
    /// Идентификатор пакета
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название пакета
    /// </summary>
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    
    /// <summary>
    /// Тип пакета
    /// </summary>
    [Column("type", TypeName = "varchar(64)")]
    public PackageType Type { get; set; }
    
    /// <summary>
    /// Цена пакета
    /// </summary>
    [Column("price", TypeName = "numeric")]
    public double Price { get; set; }
    
    /// <summary>
    /// Общее время проведения по пакету
    /// </summary>
    [Column("rental_time")]
    public int RentalTime { get; set; }
    
    /// <summary>
    /// Описание пакета
    /// </summary>
    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    /// <summary>
    /// Зоны, которые входят в пакет
    /// </summary>
    public ICollection<ZoneDbModel> Zones { get; set; }
    
    /// <summary>
    /// Меню блюд пакета
    /// </summary>
    public ICollection<DishDbModel> Dishes { get; set; }

    public PackageDbModel(Guid id, string name, PackageType type, double price, int rentalTime,
        string description)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        RentalTime = rentalTime;
        Description = description;
        Dishes = new List<DishDbModel>();
        Zones = new List<ZoneDbModel>();
    }

    public PackageDbModel(Guid id, string name, PackageType type, double price, int rentalTime,
        string description, ICollection<DishDbModel> dishes)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        RentalTime = rentalTime;
        Description = description;
        Dishes = dishes;
        Zones = new List<ZoneDbModel>();
    }

    public PackageDbModel(Guid id, string name, PackageType type, double price, int rentalTime, 
        string description, ICollection<DishDbModel> dishes, ICollection<ZoneDbModel> zones)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        RentalTime = rentalTime;
        Description = description;
        Dishes = dishes;
        Zones = zones;
    }
}
