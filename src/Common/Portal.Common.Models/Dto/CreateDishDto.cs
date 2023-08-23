using Portal.Common.Models.Enums;

namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для создания блюда меню
/// </summary>
public class CreateDishDto
{
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

    public CreateDishDto(string name, DishType type, double price, string description)
    {
        Name = name;
        Type = type;
        Price = price;
        Description = description;
    }
}