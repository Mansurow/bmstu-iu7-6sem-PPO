using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Database.Models;

/// <summary>
/// Модель базы данных зона
/// </summary>
[Table("zones")]
public class ZoneDbModel
{
    /// <summary>
    /// Идентификатор зоны
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название зоны
    /// </summary>
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    
    /// <summary>
    /// Адрес зоны
    /// </summary>
    [Column("address")]
    public string Address { get; set; }
    
    /// <summary>
    /// Размер зоны в кв. метрах
    /// </summary>
    [Column("size")]
    public double Size { get; set; }
    
    /// <summary>
    /// Максимальное количество людей  
    /// </summary>
    [Column("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// Рейнтинг зоны по отзывам
    /// </summary>
    [Column("rating", TypeName = "numeric")]
    public double Rating { get; set; }
    
    /// <summary>
    /// Список инвентаря
    /// </summary>
    public ICollection<InventoryDbModel> Inventories { get; set; }
    
    /// <summary>
    /// Список пакетов
    /// </summary>
    public ICollection<PackageDbModel> Packages { get; set; }

    public ZoneDbModel(Guid id, string name, string address, double size, int limit, double rating)
    {
        Id = id;
        Name = name;
        Address = address;
        Size = size;
        Limit = limit;
        Rating = rating;
        Inventories = new List<InventoryDbModel>();
        Packages = new List<PackageDbModel>();
    }

    public ZoneDbModel(Guid id, string name, string address, double size, int limit, double rating, 
        ICollection<InventoryDbModel> inventories)
    {
        Id = id;
        Name = name;
        Address = address;
        Size = size;
        Limit = limit;
        Rating = rating;
        Inventories = inventories;
        Packages = new List<PackageDbModel>();  
    }

    public ZoneDbModel(Guid id, string name, string address, double size, int limit, double rating, 
        ICollection<InventoryDbModel> inventories, ICollection<PackageDbModel> packages)
    {
        Id = id;
        Name = name;
        Address = address;
        Size = size;
        Limit = limit;
        Rating = rating;
        Inventories = inventories;
        Packages = packages;
    }
}
