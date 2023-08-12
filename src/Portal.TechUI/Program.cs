using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.Database.Context;
using Portal.Database.Repositories.Interfaces;
using Portal.Database.Repositories.NpgsqlRepositories;
using Portal.Services.BookingService;
using Portal.Services.FeedbackService;
using Portal.Services.InventoryServices;
using Portal.Services.MenuService;
using Portal.Services.OauthService;
using Portal.Services.PackageService;
using Portal.Services.UserService;
using Portal.Services.ZoneService;

namespace Portal.TechUI;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
        {
            // services.AddSingleton<IDbContextFactory, PgSQLDbContextFactory>();

            services.AddDbContext<PortalDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("default"));
            });

            services.AddSingleton(config);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IZoneRepository, ZoneRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();

            services.AddScoped<UserService>();
            services.AddScoped<BookingService>();
            services.AddScoped<FeedbackService>();
            services.AddScoped<OauthService>();
            services.AddScoped<MenuService>();
            services.AddScoped<ZoneService>();
            services.AddScoped<InventoryService>();
            services.AddScoped<PackageService>();

            services.AddSingleton<Startup>();
        });

        var host = builder.Build();

        await using var context = host.Services.GetRequiredService<PortalDbContext>();
        await context.Database.MigrateAsync();

        using (var serviceScope = host.Services.CreateAsyncScope())
        {
            var services = serviceScope.ServiceProvider;
            try
            {
                Console.WriteLine("Запуск приложения...");
                var techUI = services.GetRequiredService<Startup>();
                await techUI.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Приложение приостановлено.");
            }
        }
    }
}