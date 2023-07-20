using Portal.Common.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Portal.Database.Models;
public class BookingDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [ForeignKey("Zone")]
    [Column("zone_id")]
    public Guid ZoneId { get; set; }
    public ZoneDbModel? Zone { get; set; }

    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    public UserDbModel? User { get; set; }

    [ForeignKey("Package")]
    [Column("package_id")]
    public Guid PackageId { get; set; }
    public PackageDbModel Package { get; set; }
    
    [Column("amount_of_people", TypeName = "integer")]
    public int AmountPeople { get; set; }
    [Column("status", TypeName = "integer")]
    public BookingStatus Status { get; set; }
    [Column ("date")]
    public DateOnly Date { get; set; }
    [Column("start_time")]
    public TimeOnly StartTime { get; set; }
    [Column("end_time")]
    public TimeOnly EndTime { get; set; }

    public BookingDbModel(Guid id, Guid zoneId, Guid userId, Guid packageId,
        int amountPeople, DateOnly date, TimeOnly startTime, TimeOnly endTime, BookingStatus status)
    {
        Id = id;
        ZoneId = zoneId;
        UserId = userId;
        PackageId = packageId;
        AmountPeople = amountPeople;
        Status = status;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }
}