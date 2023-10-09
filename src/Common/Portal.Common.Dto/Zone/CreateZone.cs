using System.ComponentModel.DataAnnotations;
using Portal.Common.Dto.Inventory;

namespace Portal.Common.Dto.Zone;

/// <summary>
/// Модель Dto для создания зоны
/// </summary>
public class CreateZone
{
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
    /// Инвентарь зоны
    /// </summary>
    [Required]
    public List<CreateInventory> Inventories { get; set; }
    
    /// <summary>
    /// Пакеты зоны
    /// </summary>
    [Required]
    public List<Guid> Packages { get; set; }
    
    public CreateZone(string name, string address, int limit, double size, List<CreateInventory> inventories, List<Guid> packages)
    {
        Name = name;
        Address = address;
        Limit = limit;
        Size = size;
        Inventories = inventories;
        Packages = packages;
    }
}