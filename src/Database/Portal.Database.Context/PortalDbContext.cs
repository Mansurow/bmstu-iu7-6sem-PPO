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
            .WithMany(p => p.Zones);

        modelBuilder.Entity<PackageDbModel>()
            .HasMany(p => p.Dishes)
            .WithMany(d => d.Packages);

        base.OnModelCreating(modelBuilder);
    }
}
