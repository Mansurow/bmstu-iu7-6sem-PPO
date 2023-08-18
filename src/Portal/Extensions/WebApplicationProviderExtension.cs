using Portal.Database.Context;
using Microsoft.EntityFrameworkCore;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Configuration;
using Portal.Database.Repositories.NpgsqlRepositories;
using Portal.Services.OauthService;
using Portal.Services.UserService;
using Serilog;

namespace Portal.Extensions;

public static class WebApplicationProviderExtension
{
    public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
    {
        await using var context = app.Services.GetRequiredService<PortalDbContext>();
        await context.Database.MigrateAsync();
        
        return app;
    }

    // TODO: Починить добавления админа по запуску
    public static async Task<WebApplication> AddPortalAdministrator(this WebApplication app)
    {
        var adminOptions = app.Configuration.GetSection("AdministratorConfiguration").Get<AdministratorConfiguration>();
        var service = app.Services.GetRequiredService<UserService>();

        if (adminOptions is null)
        {
            await service.CreateAdmin("admin@gmail.com", "admin");
        }
        else
        {
            await service.CreateAdmin(adminOptions.Login, adminOptions.Password);
        }
        
        return app;
    }
}