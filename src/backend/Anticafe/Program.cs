using System.Text.Json.Serialization;
using Anticafe.BL.Services.MenuService;
using Anticafe.BL.Sevices.BookingService;
using Anticafe.BL.Sevices.FeedbackService;
using Anticafe.BL.Sevices.OauthService;
using Anticafe.BL.Sevices.RoomService;
using Anticafe.BL.Sevices.UserService;
using Anticafe.DataAccess;
using Anticafe.DataAccess.IRepositories;
using Anticafe.PostgreSQL.Repositories;
using Anticafe.MongoDB.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NLog.Web;
using Anticafe.MongoDB;
using Anticafe.PostgreSQL;

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
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var db = builder.Configuration.GetSection("App").GetSection("UseDb").Value;

if (db is not null && db == "Mongo")
{
    var client = new MongoClient(builder.Configuration.GetSection("MongoDB").GetConnectionString("default"));
    builder.Services.AddSingleton<IMongoClient>(client);
    builder.Services.AddSingleton<IDbCollectionFactory, MongoCollectionFactory>();

    builder.Services.AddScoped<IUserRepository, Anticafe.MongoDB.Repositories.UserRepository>();
    builder.Services.AddScoped<IBookingRepository, Anticafe.MongoDB.Repositories.BookingRepository>();
    builder.Services.AddScoped<IFeedbackRepository, Anticafe.MongoDB.Repositories.FeedbackRepository>();
    builder.Services.AddScoped<IRoomRepository, Anticafe.MongoDB.Repositories.RoomRepository>();
    builder.Services.AddScoped<IMenuRepository, Anticafe.MongoDB.Repositories.MenuRepository>();
} 
else
{
    builder.Services.AddDbContext<PgSQLDbContext>(option => option.UseNpgsql(builder.Configuration.GetSection("PostgreSQL").GetConnectionString("default")));
    builder.Services.AddSingleton<Anticafe.PostgreSQL.IDbContextFactory<PgSQLDbContext>, PgSQLDbContextFactory>();

    builder.Services.AddScoped<IUserRepository, Anticafe.PostgreSQL.Repositories.UserRepository>();
    builder.Services.AddScoped<IBookingRepository, Anticafe.PostgreSQL.Repositories.BookingRepository>();
    builder.Services.AddScoped<IFeedbackRepository, Anticafe.PostgreSQL.Repositories.FeedbackRepository>();
    builder.Services.AddScoped<IRoomRepository, Anticafe.PostgreSQL.Repositories.RoomRepository>();
    builder.Services.AddScoped<IMenuRepository, Anticafe.PostgreSQL.Repositories.MenuRepository>();
}


builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

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
