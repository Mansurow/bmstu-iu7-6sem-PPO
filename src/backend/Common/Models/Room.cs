namespace Anticafe.BL.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
    public double Price { get; set; }
    public double Rating { get; set; }
    public ICollection<Inventory>? Inventories { get; set; }

    public Room(int id, string name, int size, double price, double rating, ICollection<Inventory>? inventories)
    {
        Id = id;
        Name = name;
        Size = size;
        Price = price;
        Rating = rating;
        Inventories = inventories;
    }

    public void ChangeRating(double rating)
    {
        Rating = rating;
    }
    
    public void ChangePrice(double price)
    {
        Price = price;
    }

    public void AddInventory(Inventory newInventory) 
    {
        if (Inventories == null)
            Inventories = new List<Inventory>() { newInventory };
        else
            Inventories.Add(newInventory);
    }
}
