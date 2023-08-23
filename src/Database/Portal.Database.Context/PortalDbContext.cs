using Microsoft.EntityFrameworkCore;
using Portal.Database.Models;

namespace Portal.Database.Context;

/// <summary>
/// Контекст база данных
/// </summary>
public class PortalDbContext : DbContext
{
    /// <summary>
    /// Таблица пользователй
    /// </summary>
    public DbSet<UserDbModel> Users { get; set; }
    
    /// <summary>
    /// Таблица пакетов
    /// </summary>
    public DbSet<PackageDbModel> Packages { get; set; }
    
    /// <summary>
    /// Таблица броней
    /// </summary>
    public DbSet<BookingDbModel> Bookings { get; set; }
    
    /// <summary>
    /// Таблица отзывов
    /// </summary>
    public DbSet<FeedbackDbModel> Feedbacks { get; set; }
    
    /// <summary>
    /// Таблица инвентаря
    /// </summary>
    public DbSet<InventoryDbModel> Inventories { get; set; }
    
    /// <summary>
    /// Таблица зон
    /// </summary>
    public DbSet<ZoneDbModel> Zones { get; set; }
    
    /// <summary>
    /// Таблица меню (блюд)
    /// </summary>
    public DbSet<DishDbModel> Menu { get; set; }

    public PortalDbContext(DbContextOptions<PortalDbContext> options): base(options) { }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=anticafe;User Id=postgres;Password=postgres;");
    }*/
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ZoneDbModel>()
            .HasMany(r => r.Packages)
            .WithMany(p => p.Zones)
            .UsingEntity("ZonePackages",
                p => p.HasOne(typeof(PackageDbModel)).WithMany().HasForeignKey("package_id").HasPrincipalKey(nameof(PackageDbModel.Id)),
                z => z.HasOne(typeof(ZoneDbModel)).WithMany().HasForeignKey("zone_id").HasPrincipalKey(nameof(ZoneDbModel.Id)),
                j => j.HasKey("package_id", "zone_id"));

        modelBuilder.Entity<PackageDbModel>()
            .HasMany(p => p.Dishes)
            .WithMany(d => d.Packages)
            .UsingEntity("PackageDishes",
                d => d.HasOne(typeof(DishDbModel)).WithMany().HasForeignKey("dish_id").HasPrincipalKey(nameof(DishDbModel.Id)),
                p => p.HasOne(typeof(PackageDbModel)).WithMany().HasForeignKey("package_id").HasPrincipalKey(nameof(PackageDbModel.Id)),
                j => j.HasKey("dish_id", "package_id"));

        base.OnModelCreating(modelBuilder);
    }
}
