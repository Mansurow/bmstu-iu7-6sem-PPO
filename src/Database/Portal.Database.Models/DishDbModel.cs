using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Common.Models.Enums;

namespace Portal.Database.Models;

/// <summary>
/// Модель базы данных блюдо
/// </summary>
[Table("dishes")]
public class DishDbModel
{
    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название блюда
    /// </summary>
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    
    /// <summary>
    /// Тип блюда
    /// </summary>
    [Column("type", TypeName = "varchar(64)")]
    public DishType Type { get; set; }
    
    /// <summary>
    /// Цена блюда
    /// </summary>
    [Column("price")]
    public double Price { get; set; }
    
    /// <summary>
    /// Описание блюда
    /// </summary>
    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    public ICollection<PackageDbModel> Packages { get; set; }
    
    public DishDbModel(Guid id, string name, DishType type, double price, string description)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        Description = description;
        Packages = new HashSet<PackageDbModel>();
    }

    public DishDbModel(Guid id, string name, DishType type, double price, string description, ICollection<PackageDbModel> packages) 
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        Description = description;
        Packages = packages;
    }
}
