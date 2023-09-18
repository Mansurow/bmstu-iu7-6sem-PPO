using Portal.Common.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Portal.Database.Models;

/// <summary>
/// Модель базы данных брони
/// </summary>
[Table("bookings")]
public class BookingDbModel
{
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор зоны
    /// </summary>
    [ForeignKey("Zone")]
    [Column("zone_id")]
    public Guid ZoneId { get; set; }
    /// <summary>
    /// Зона
    /// </summary>
    public ZoneDbModel? Zone { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    /// <summary>
    /// Пользователь
    /// </summary>
    public UserDbModel? User { get; set; }

    /// <summary>
    /// Идентификатор пакета
    /// </summary>
    [ForeignKey("Package")]
    [Column("package_id")]
    public Guid PackageId { get; set; }
    /// <summary>
    /// Пакет
    /// </summary>
    public PackageDbModel? Package { get; set; }
    
    /// <summary>
    /// Количество людей 
    /// </summary>
    [Column("amount_of_people", TypeName = "integer")]
    public int AmountPeople { get; set; }
    
    /// <summary>
    /// Статус брони
    /// </summary>
    [Column("status", TypeName = "varchar(64)")]
    public BookingStatus Status { get; set; }
    
    /// <summary>
    /// Статус брони
    /// </summary>
    [Column ("date")]
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Время брони - начало
    /// </summary>
    [Column("start_time")]
    public TimeOnly StartTime { get; set; }
    
    /// <summary>
    /// Время брони - конец
    /// </summary>
    [Column("end_time")]
    public TimeOnly EndTime { get; set; }
    
    /// <summary>
    /// Время создания брони
    /// </summary>
    [Column("create_date_time")]
    public DateTime CreateDateTime { get; set; }

    /// <summary>
    /// Оплачена ли бронь
    /// </summary>
    [Column("is_paid")]
    public bool IsPaid { get; set; }
    
    /// <summary>
    /// Общая стоимоcть брони
    /// </summary>
    [Column("total_price", TypeName = "numeric")]
    public double TotalPrice { get; set; }
    
    public BookingDbModel(Guid id, Guid zoneId, Guid userId, Guid packageId,
        int amountPeople, DateOnly date, TimeOnly startTime, TimeOnly endTime, DateTime createDateTime, 
        double totalPrice, BookingStatus status, bool isPaid)
    {
        Id = id;
        ZoneId = zoneId;
        UserId = userId;
        PackageId = packageId;
        AmountPeople = amountPeople;
        Status = status;
        Date = date;
        CreateDateTime = createDateTime;
        StartTime = startTime;
        EndTime = endTime;
        IsPaid = isPaid;
        TotalPrice = totalPrice;
    }
}