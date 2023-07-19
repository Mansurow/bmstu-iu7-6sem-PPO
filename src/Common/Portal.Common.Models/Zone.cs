namespace Portal.Common.Models;

public class Zone
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int Limit { get; set; }
    public double Size { get; set; }
    public double Price { get; set; }
    public double Rating { get; set; }
    public ICollection<Inventory> Inventories { get; set; }
    public ICollection<Package> Packages { get; set; }


    public Zone(Guid id, string name, string address, double size, int limit, double price, double rating)
    {
        Id = id;
        Name = name;
        Address = address;
        Size = size;
        Limit = limit;
        Price = price;
        Rating = rating;
        Inventories = new List<Inventory>();
        Packages = new List<Package>();
    }

    public Zone(Guid id, string name, string address, double size, int limit, double price, double rating, ICollection<Inventory> inventories, ICollection<Package> packages)
    {
        Id = id;
        Name = name;
        Address = address;
        Size = size;
        Limit = limit;
        Price = price;
        Rating = rating;
        Inventories = inventories;
        Packages = packages;
    }

    public Zone(Guid id, string name, string address, int size, int limit, double price, double rating, ICollection<Inventory> inventories)
    {
        Id = id;
        Name = name;
        Size = size;
        Address = address;
        Limit = limit;
        Price = price;
        Rating = rating;
        Inventories = inventories;
        Packages = new List<Package>();
    }

    public void ChangeRating(double rating)
    {
        Rating = rating;
    }

    public void ChangePrice(double price)
    {
        Price = price;
    }

    public void ChangeAddress(string address)
    {
        Address = address;
    }

    public void AddInventory(Inventory newInventory)
    {
        Inventories.Add(newInventory);
    }
}
