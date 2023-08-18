namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для обновления зоны
/// </summary>
public class UpdateZoneDto
{
    public UpdateZoneDto(Guid id, string name, string address, int limit, double size, double price, ICollection<Package> packages, ICollection<Inventory> inventories)
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

    /// <summary>
    /// Идентификатор зоны 
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название зоны
    /// </summary>
    /// <example>Космос</example>
    public string Name { get; set; }
    
    /// <summary>
    /// Адрес зоны
    /// </summary>
    /// <example>г.Москва, Рубцовская набережная 13/2 (м. Электрозаводская)</example>
    public string Address { get; set; }
    
    /// <summary>
    ///  Ограничение на количество людей в зоне
    /// </summary>
    /// <example>16</example>
    public int Limit { get; set; }
    
    /// <summary>
    /// Размер зоны в кв. метрах
    /// </summary>
    /// <example>25.0</example>
    public double Size { get; set; }
    
    /// <summary>
    /// Стоимость зоны за час в рублях
    /// </summary>
    /// <example>349.99</example>
    public double Price { get; set; }
    
    /// <summary>
    /// Инвентарь зоны
    /// </summary>
    public ICollection<Inventory> Inventories { get; set; }
    
    /// <summary>
    /// Пакеты зоны
    /// </summary>
    public ICollection<Package> Packages { get; set; }
}