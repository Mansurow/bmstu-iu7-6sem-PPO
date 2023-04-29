using Microsoft.EntityFrameworkCore;
using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess;

public partial class AppDbContext : DbContext
{
    public DbSet<UserDbModel> Users { get; set; }
    public DbSet<BookingDbModel> Bookings { get; set; }
    public DbSet<FeedbackDbModel> Feedbacks { get; set; }
    public DbSet<InventoryDbModel> Inventories { get; set; }
    public DbSet<RoomDbModel> Rooms { get; set; }
    public DbSet<MenuDbModel> Menu { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=anticafe;User Id=postgres;Password=postgres;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomDbModel>()
                    .HasMany(r => r.Inventories)
                    .WithMany(i => i.Rooms);

        base.OnModelCreating(modelBuilder);
    }
}
