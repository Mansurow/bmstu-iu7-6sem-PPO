using System.Runtime.Serialization;

namespace Portal.Common.Models.Enums;

/// <summary>
/// Тип блюда
/// </summary>
public enum DishType
{
    /// <summary>
    /// Первые блюда
    /// </summary>
    [EnumMember(Value = "Первые блюда")]
    FirstCourse = 1,
    
    /// <summary>
    /// Вторые блюда
    /// </summary>
    [EnumMember(Value = "Вторые блюда")]
    SecondCourse = 2,
    
    /// <summary>
    /// Напитки
    /// </summary>
    [EnumMember(Value = "Напитки")]
    Drinks = 3,
    
    /// <summary>
    /// Холодные блюда
    /// </summary>
    [EnumMember(Value = "Холодные закуски")] 
    ColdSnacks = 4,
    
    /// <summary>
    /// Горячые блюда
    /// </summary>
    [EnumMember(Value = "Горячие закуски")] 
    HotSnacks = 5,
    
    /// <summary>
    /// Салаты
    /// </summary>
    [EnumMember(Value = "Салаты")] 
    Salat = 6,
    
    /// <summary>
    /// Десерты
    /// </summary>
    [EnumMember(Value = "Десерты")] 
    Desserts = 7,
    
    /// <summary>
    /// Десерты
    /// </summary>
    [EnumMember(Value = "Разное")] 
    Others = 8
}
