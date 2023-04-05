namespace Anticafe.BL.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
    public double Price { get; set; }
    public double Rating { get; set; }
    public ICollection<Inventory>? Inventories { get; set; }
    public ICollection<Menu>? Menu { get; set; }

    Room(int id, string name, int size, double price, double rating, ICollection<Inventory>? inventories, ICollection<Menu>? menu)
    {
        Id = id;
        Name = name;
        Size = size;
        Price = price;
        Rating = rating;
        Inventories = inventories;
        Menu = menu;
    }
}
