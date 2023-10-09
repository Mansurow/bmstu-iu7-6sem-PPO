using System.ComponentModel.DataAnnotations;
using Portal.Common.Enums;

namespace Portal.Common.Dto.Package;

/// <summary>
/// Модель для создания пакета
/// </summary>
public class CreatePackage
{
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
    /// Блюда пакета - идентификаторы для добавления
    /// </summary>
    [Required]
    public List<Guid> Dishes { get; set; }
    
    public CreatePackage(string name, PackageType type, double price, int rentalTime, string description, List<Guid> dishes)
    {
        Name = name;
        Type = type;
        Price = price;
        RentalTime = rentalTime;
        Description = description;
        Dishes = dishes;
    }
}