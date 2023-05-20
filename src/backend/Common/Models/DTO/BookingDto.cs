using Anticafe.Common.Enums;
using Newtonsoft.Json;

namespace Common.Models.DTO;

[JsonObject]
public class BookingDto
{
    [JsonProperty("bookingId")]
    public int Id { get; set; }
    [JsonProperty("roomId")]
    public int RoomId { get; set; }
    [JsonProperty("userId")]
    public int UserId { get; set; }
    [JsonProperty("amountPeople")]
    public int AmountPeople { get; set; }
    [JsonProperty("status")]
    public BookingStatus Status { get; set; }
    [JsonProperty("startTime")]
    public string StartTime { get; set; }
    [JsonProperty("endTime")]
    public string EndTime { get; set; }

    public BookingDto(int id, int roomId, int userId, int amountPeople, BookingStatus status, string startTime, string endTime)
    {
        Id = id;
        RoomId = roomId;
        UserId = userId;
        AmountPeople = amountPeople;
        Status = status;
        StartTime = startTime;
        EndTime = endTime;
    }
}
