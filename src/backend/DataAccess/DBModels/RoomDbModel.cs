using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Anticafe.DataAccess.DBModels;

public class RoomDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    [Column("size")]
    public int Size { get; set; }
    [Column("price")]
    public double Price { get; set; }
    [Column("Raiting", TypeName = "numeric")]
    public double Rating { get; set; }
    public ICollection<InventoryDbModel>? Inventories { get; set; }

    public RoomDbModel(int id, string name, int size, double price, double rating)
    {
        Id = id;
        Name = name;
        Size = size;
        Price = price;
        Rating = rating;
        Inventories = null;
    }

    public RoomDbModel(int id, string name, int size, double price, double rating, ICollection<InventoryDbModel>? inventories)
    {
        Id = id;
        Name = name;
        Size = size;
        Price = price;
        Rating = rating;
        Inventories = inventories;
    }
}
