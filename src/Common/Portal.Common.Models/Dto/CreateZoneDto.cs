namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель Dto для создания зоны
/// </summary>
public class CreateZoneDto
{
    public CreateZoneDto(string name, string address, int limit, double size, List<CreateInventoryDto> inventories, List<Guid> packages)
    {
        Name = name;
        Address = address;
        Limit = limit;
        Size = size;
        Inventories = inventories;
        Packages = packages;
    }

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
    /// Инвентарь зоны
    /// </summary>
    public List<CreateInventoryDto> Inventories { get; set; }
    
    /// <summary>
    /// Пакеты зоны
    /// </summary>
    public List<Guid> Packages { get; set; }
}