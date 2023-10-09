using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Dto.Zone;

public class Zone
{
    /// <summary>
    /// Идентификатор зоны 
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название зоны
    /// </summary>
    /// <example>Космос</example>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Адрес зоны
    /// </summary>
    /// <example>г.Москва, Рубцовская набережная 13/2 (м. Электрозаводская)</example>
    [Required]
    public string Address { get; set; }
    
    /// <summary>
    ///  Ограничение на количесво людей в зоне
    /// </summary>
    /// <example>16</example>
    [Required]
    public int Limit { get; set; }
    
    /// <summary>
    /// Размер зоны в кв. метрах
    /// </summary>
    /// <example>25.0</example>
    [Required]
    public double Size { get; set; }

    /// <summary>
    /// Рейнтинг зоны по отзывам пользователей
    /// </summary>
    /// <example>5.0</example>
    [Required]
    public double Rating { get; set; }
    
    /// <summary>
    /// Инвентарь зоны
    /// </summary>
    [Required]
    public ICollection<Inventory.Inventory> Inventories { get; set; }
    
    /// <summary>
    /// Пакеты зоны
    /// </summary>
    [Required]
    public ICollection<Package.Package> Packages { get; set; }
    
    public Zone(Guid id, string name, string address, double size, int limit, 
        double rating, ICollection<Inventory.Inventory> inventories, ICollection<Package.Package> packages)
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