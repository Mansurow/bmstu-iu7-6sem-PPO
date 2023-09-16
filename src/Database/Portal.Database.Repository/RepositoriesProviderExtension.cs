using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portal.Database.Core.Repositories;
using Portal.Database.Repositories.NpgsqlRepositories;

namespace Portal.Database.Repositories;

/// <summary>
/// Провайдер для инверсии зависимостей репозиториев  
/// </summary>
public static class RepositoriesProviderExtension
{
    /// <summary>
    /// Инверсия зависимостей репозиториев
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="configuration">Конфигурация приложения</param>
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