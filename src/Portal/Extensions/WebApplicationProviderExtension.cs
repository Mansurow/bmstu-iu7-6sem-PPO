using Portal.Database.Context;
using Microsoft.EntityFrameworkCore;
using Portal.Configuration;
using Portal.Services.UserService;

namespace Portal.Extensions;

public static class WebApplicationProviderExtension
{
    public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
    {
        await using var context = app.Services.GetRequiredService<PortalDbContext>();
        await context.Database.MigrateAsync();
        
        return app;
    }
    
    public static async Task<WebApplication> AddPortalAdministrator(this WebApplication app)
    {
        var adminOptions = app.Configuration.GetSection("AdministratorConfiguration").Get<AdministratorConfiguration>();

        using var serviceScope = app.Services.CreateScope();
        var services = serviceScope.ServiceProvider;
        var userService = services.GetRequiredService<IUserService>();
            
        if (adminOptions is null)
        {
            await userService.CreateAdmin("admin","admin");
        }
        else
        {
            await userService.CreateAdmin(adminOptions.Login, adminOptions.Password);
        }

        return app;
    }
}