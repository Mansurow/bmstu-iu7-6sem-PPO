using System.Diagnostics;
using Anticafe.BL.Services.MenuService;
using Anticafe.BL.Sevices.BookingService;
using Anticafe.BL.Sevices.FeedbackService;
using Anticafe.BL.Sevices.OauthService;
using Anticafe.BL.Sevices.RoomService;
using Anticafe.BL.Sevices.UserService;
using Anticafe.DataAccess.IRepositories;
using Anticafe.PostgreSQL;
using Anticafe.PostgreSQL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Anticafe.TechUI;

internal static class Program 
{
    [STAThread]
    static void Main() 
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<IDbContextFactory<PgSQLDbContext>, PgSQLDbContextFactory>();

            services.AddSingleton(config);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            services.AddScoped<UserService>();
            services.AddScoped<BookingService>();
            services.AddScoped<FeedbackService>();
            services.AddScoped<Oauthservice>();
            services.AddScoped<MenuService>();
            services.AddScoped<RoomService>();

            services.AddSingleton<TechUI>();
        });

        var host = builder.Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            try
            {
                Console.WriteLine("Запуск приложения...");
                var techUI = services.GetRequiredService<TechUI>();
                techUI.Run();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}

