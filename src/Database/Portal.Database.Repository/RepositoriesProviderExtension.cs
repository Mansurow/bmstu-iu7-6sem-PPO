using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portal.Database.Repositories.Interfaces;
using Portal.Database.Repositories.NpgsqlRepositories;

namespace Portal.Database.Repositories;

public static class RepositoriesProviderExtension
{
    public static void AddPortalNpgsqlRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IZoneRepository, ZoneRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
    }
}