using Portal.Common.Models.Enums;

namespace Portal.Common.Models;

/// <summary>
/// Блюдо меню
/// </summary>
public class Dish
{
    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название блюда
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Тип блюда
    /// </summary>
    public DishType Type { get; set; }
    
    /// <summary>
    /// Цена блюда в рублях (для отдельного заказа)
    /// </summary>
    public double Price { get; set; }
    
    /// <summary>
    /// Описание блюда
    /// </summary>
    public string Description { get; set; }

    public Dish(Guid id, string name, DishType type, double price, string description)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        Description = description;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        var other = (Dish) obj;
        return Id == other.Id
               && Name == other.Name
               && Type == other.Type
               && Math.Abs(Price - other.Price) < 1e-8
               && Description == other.Description;
    }
}