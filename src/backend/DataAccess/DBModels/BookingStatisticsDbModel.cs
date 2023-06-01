using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anticafe.DataAccess.DBModels;

[Table("BookingStatistics")]
public class BookingStatisticsDbModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [ForeignKey("Room")]
    [Column("room_id")]
    public int RoomId { get; set; }
    [Column("avg_duration")]
    public double AvgDuration { get; set; }
    [Column("max_duration")]
    public double MaxDuration { get; set; }
    [Column("total_count")]
    public int TotalCount { get; set; }
    public RoomDbModel? Room { get; set; }

    public BookingStatisticsDbModel(int id, int roomId, double avgDuration, double maxDuration, int totalCount)
    {
        Id = id;
        RoomId = roomId;
        AvgDuration = avgDuration;
        MaxDuration = maxDuration;
        TotalCount = totalCount;
    }
}
