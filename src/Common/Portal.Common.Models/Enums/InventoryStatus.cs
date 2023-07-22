namespace Portal.Common.Models.Enums;

/// <summary>
/// Состояние инвентаря
/// </summary>
public enum InventoryStatus
{
    /// <summary>
    /// Новый 
    /// </summary>
    New = 1,
    
    /// <summary>
    /// Используется
    /// </summary>
    Used = 2,
    
    /// <summary>
    /// На списании
    /// </summary>
    Decommissioned = 3
}
