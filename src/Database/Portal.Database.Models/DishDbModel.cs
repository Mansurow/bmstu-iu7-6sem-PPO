using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Common.Models.Enums;

namespace Portal.Database.Models;

public class DishDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    [Column("type", TypeName = "varchar(64)")]
    public DishType Type { get; set; }
    [Column("price")]
    public double Price { get; set; }
    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    public DishDbModel(Guid id, string name, DishType type, double price, string description)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        Description = description;
    }
}
