using System.ComponentModel.DataAnnotations;
using Portal.Common.Enums;

namespace Portal.Common.Dto.Package;

public class Package
{
    /// <summary>
    /// Идентификатор 
    /// </summary>
    [Required]
    public Guid Id { get; set; }
        
    /// <summary>
    /// Название пакета
    /// </summary>
    [Required]
    public string Name { get; set; }
        
    /// <summary>
    /// Тип пакета
    /// </summary>
    [Required]
    public PackageType Type { get; set; }
        
    /// <summary>
    /// Цена пакета
    /// </summary>
    [Required]
    public double Price { get; set; }
        
    /// <summary>
    /// Общее время проведения
    /// </summary>
    [Required]
    public int RentalTime { get; set; }
        
    /// <summary>
    /// Описания пакета
    /// </summary>
    [Required]
    public string Description { get; set; }
        
    /// <summary>
    /// Зоны, в которых доступен данный пакет
    /// </summary>
    [Required]
    public ICollection<Zone.Zone> Zones { get; set; }
        
    /// <summary>
    /// Включенный список блюд в пакет 
    /// </summary>
    [Required]
    public ICollection<Dish.Dish> Dishes { get; set; }
    
    public Package(Guid id, string name, PackageType type, double price, int rentalTime,
        string description, ICollection<Zone.Zone> zones, ICollection<Dish.Dish> dishes)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        RentalTime = rentalTime;
        Description = description;
        Zones = zones;
        Dishes = dishes;
    }
}