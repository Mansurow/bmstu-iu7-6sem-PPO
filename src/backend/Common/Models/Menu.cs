using Anticafe.Common.Enums;

namespace Anticafe.BL.Models;

public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DishType Type { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }

    public Menu (int id, string name, DishType type, double price, string description)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        Description = description;
    }
}