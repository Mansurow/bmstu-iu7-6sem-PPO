namespace Anticafe.BL.Models;

public class Feedback
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public DateTime Date { get; set; }
    public int Mark { get; set; }
    public string? Message { get; set; }

    Feedback(int id, int userId, int roomId, DateTime date, int mark, string? message)
    {
        Id = id;
        UserId = userId;
        RoomId = roomId;
        Date = date;
        Mark = mark;
        Message = message;
    }
}
