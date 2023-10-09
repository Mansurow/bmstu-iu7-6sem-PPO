using System.ComponentModel.DataAnnotations;
using Portal.Common.Enums;

namespace Portal.Common.Dto.Dish;

/// <summary>
/// Модель для создания блюда меню
/// </summary>
public class CreateDish
{
    /// <summary>
    /// Название блюда
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Тип блюда
    /// </summary>
    [Required]
    public DishType Type { get; set; }
    
    /// <summary>
    /// Цена блюда в рублях (для отдельного заказа)
    /// </summary>
    [Required]
    public double Price { get; set; }
    
    /// <summary>
    /// Описание блюда
    /// </summary>
    [Required]
    public string Description { get; set; }

    public CreateDish(string name, DishType type, double price, string description)
    {
        Name = name;
        Type = type;
        Price = price;
        Description = description;
    }
}