using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Database.Models;

public class InventoryDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    [Column ("description", TypeName = "text")]
    public string Description { get; set; }
    [Column("year_of_production")]
    public DateOnly YearOfProduction { get; set; }

    public InventoryDbModel(Guid id, string name, string description, DateOnly yearOfProduction)
    {
        Id = id;
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
    }
}
