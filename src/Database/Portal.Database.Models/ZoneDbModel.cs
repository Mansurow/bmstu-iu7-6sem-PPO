using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Database.Models;

public class ZoneDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    [Column("address")]
    public string Address { get; set; }
    [Column("size")]
    public double Size { get; set; }
    [Column("limit")]
    public int Limit { get; set; }
    [Column("price")]
    public double Price { get; set; }
    [Column("Raiting", TypeName = "numeric")]
    public double Rating { get; set; }
    public ICollection<InventoryDbModel> Inventories { get; set; }
    public ICollection<PackageDbModel> Packages { get; set; }

    public ZoneDbModel(Guid id, string name, string address, double size, int limit, double price, double rating, 
        ICollection<InventoryDbModel> inventories)
    {
        Id = id;
        Name = name;
        Address = address;
        Size = size;
        Limit = limit;
        Price = price;
        Rating = rating;
        Inventories = inventories;
        Packages = new List<PackageDbModel>();  
    }

    public ZoneDbModel(Guid id, string name, string address, double size, int limit, double price, double rating, 
        ICollection<InventoryDbModel> inventories, ICollection<PackageDbModel> packages)
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
}
