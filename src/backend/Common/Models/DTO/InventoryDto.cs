using Newtonsoft.Json;

namespace Common.Models.DTO;

[JsonObject]
public class InventoryDto
{
    [JsonProperty("inventoryId")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }

    public InventoryDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
