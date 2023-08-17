using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Portal.Common.Models.Dto;

[JsonObject]
public class CreateInventoryDto
{
    public CreateInventoryDto(string name, string description, string yearOfProduction)
    {
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
    }

    /// <summary>
    /// Название инветаря
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание инветаря
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Год выпуска или производства
    /// </summary>
    /// <example>2023-12-01</example>
    public string YearOfProduction { get; set; }
}