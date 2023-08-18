using Portal.Common.Models.Enums;

namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для создания пакета
/// </summary>
public class CreatePackageDto
{
    public CreatePackageDto(string name, PackageType type, double price, int rentalTime, string description, List<Guid> dishes)
    {
        Name = name;
        Type = type;
        Price = price;
        RentalTime = rentalTime;
        Description = description;
        Dishes = dishes;
    }

    /// <summary>
    /// Название пакета
    /// </summary>
    public string Name { get; set; }
        
    /// <summary>
    /// Тип пакета
    /// </summary>
    public PackageType Type { get; set; }
        
    /// <summary>
    /// Цена пакета
    /// </summary>
    public double Price { get; set; }
        
    /// <summary>
    /// Общее время проведения
    /// </summary>
    public int RentalTime { get; set; }
        
    /// <summary>
    /// Описания пакета
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Блюда пакета - идентификаторы для добавления
    /// </summary>
    public List<Guid> Dishes { get; set; }
}