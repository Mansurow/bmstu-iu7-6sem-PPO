using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Dto;

/// <summary>
/// Модель для обновления зоны
/// </summary>
public class UpdateZone
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
    ///  Ограничение на количество людей в зоне
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
    /// Стоимость зоны за час в рублях
    /// </summary>
    /// <example>349.99</example>
    [Required]
    public double Price { get; set; }
    
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
    
    public UpdateZone(Guid id, string name, string address, int limit, double size, double price, ICollection<Package.Package> packages, ICollection<Inventory.Inventory> inventories)
    {
        Id = id;
        Name = name;
        Address = address;
        Limit = limit;
        Size = size;
        Price = price;
        Packages = packages;
        Inventories = inventories;
    }
}