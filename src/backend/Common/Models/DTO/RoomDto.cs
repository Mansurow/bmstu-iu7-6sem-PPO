using Newtonsoft.Json;

namespace Common.Models.DTO;

[JsonObject]
public class RoomDto
{
    [JsonProperty("roomId")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("size")]
    public int Size { get; set; }
    [JsonProperty("price")]
    public double Price { get; set; }
    [JsonProperty("rating")]
    public double Rating { get; set; }
    [JsonProperty("inventories")]
    public ICollection<InventoryDto>? Inventories { get; set; }

    public RoomDto(int id, string name, int size, double price, double rating, ICollection<InventoryDto>? inventories)
    {
        Id = id;
        Name = name;
        Size = size;
        Price = price;
        Rating = rating;
        Inventories = inventories;
    }
}
