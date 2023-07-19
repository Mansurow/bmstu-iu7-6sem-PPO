namespace Portal.Common.Models;

public class Feedback
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime Date { get; set; }
    public double Mark { get; set; }
    public string? Message { get; set; }

    public Feedback(Guid id, Guid userId, Guid roomId, DateTime date, double mark, string? message)
    {
        Id = id;
        UserId = userId;
        RoomId = roomId;
        Date = date;
        Mark = mark;
        Message = message;
    }
}
