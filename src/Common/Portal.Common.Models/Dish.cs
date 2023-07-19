using Portal.Common.Models.Enums;

namespace Portal.Common.Models;

public class Dish
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DishType Type { get; set; }
    public double Price { get; set; }
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