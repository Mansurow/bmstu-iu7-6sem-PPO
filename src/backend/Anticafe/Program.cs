using Anticafe.BL.Services.MenuService;
using Anticafe.BL.Sevices.BookingService;
using Anticafe.BL.Sevices.FeedbackService;
using Anticafe.BL.Sevices.OauthService;
using Anticafe.BL.Sevices.RoomService;
using Anticafe.BL.Sevices.UserService;
using Anticafe.DataAccess;
using Anticafe.DataAccess.IRepositories;
using Anticafe.DataAccess.Repositories;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

// Add config
builder.Host.ConfigureAppConfiguration((_, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
});

// NLog: Setup NLog for Dependency injection
// builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddSingleton<IDbContextFactory, PgSQLDbContextFactory>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();

builder.Services.AddScoped<IOauthService, Oauthservice>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IRoomService, RoomService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

// app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.MapControllers();
app.Run();

namespace Anticafe
{
    /// <summary>
    /// Entrypoint
    /// </summary>
    public class Program
    {
    }
}
