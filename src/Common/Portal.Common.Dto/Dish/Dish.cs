using System.ComponentModel.DataAnnotations;
using Portal.Common.Enums;

namespace Portal.Common.Dto.Dish;

/// <summary>
/// Dto для получении информации о блюде меню.
/// </summary>
public class Dish
{
    /// <summary>
    /// Идентификатор блюда.
    /// </summary>
    [Required]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название блюда.
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Тип блюда.
    /// </summary>
    [Required]
    public DishType Type { get; set; }
    
    /// <summary>
    /// Цена блюда в рублях (для отдельного заказа).
    /// </summary>
    [Required]
    public double Price { get; set; }
    
    /// <summary>
    /// Описание блюда.
    /// </summary>
    [Required]
    public string Description { get; set; }

    public Dish(Guid id, string name, DishType type, double price, string description)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        Description = description;
    }
}