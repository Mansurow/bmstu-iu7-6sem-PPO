using Microsoft.EntityFrameworkCore;
using Portal.Database.Context;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PortalDbContext>(option => option.UseNpgsql(config.GetConnectionString("default")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
