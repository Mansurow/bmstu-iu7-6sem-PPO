using Newtonsoft.Json;

namespace Common.Models.DTO;

[JsonObject]
public class FeedbackDto
{
    [JsonProperty("feedbackId")]
    public int Id { get; set; }
    [JsonProperty("userId")]
    public int UserId { get; set; }
    [JsonProperty("roomId")]
    public int RoomId { get; set; }
    [JsonProperty("date")]
    public DateTime Date { get; set; }
    [JsonProperty("mark")]
    public int Mark { get; set; }
    [JsonProperty("message")]
    public string? Message { get; set; }

    public FeedbackDto(int id, int userId, int roomId, DateTime date, int mark, string? message)
    {
        Id = id;
        UserId = userId;
        RoomId = roomId;
        Date = date;
        Mark = mark;
        Message = message;
    }
}
