using System.ComponentModel.DataAnnotations;
using Anticafe.Common.Enums;
using Newtonsoft.Json;

namespace Common.Models.DTO;

[JsonObject]
public class MenuDto
{
    [JsonProperty("dishId")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }

    [EnumDataType(typeof(DishType))]
    [JsonProperty("type")]
    public DishType Type { get; set; }
    [JsonProperty("price")]
    public double Price { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }

    public MenuDto(int id, string name, DishType type, double price, string description)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        Description = description;
    }
}
