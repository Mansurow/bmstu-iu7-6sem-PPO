using Portal.Common.Converter;
using Portal.Common.Dto.Package;
using Portal.Services.PackageService;

namespace Portal.Graphql.Queries;

[ExtendObjectType("Query")]
public class PackageQuery
{
    /// <summary>
    /// Получить все пакеты.
    /// </summary>
    /// <param name="packageService"></param>
    /// <returns>Список пакеты.</returns>
    [GraphQLName("GetPackages")]
    public async Task<IEnumerable<Package>> GetPackages([Service(ServiceKind.Resolver)] IPackageService packageService)
    {
        var packages = await packageService.GetPackagesAsync();

        return packages.Select(PackageConverter.ConvertCoreToDtoModel);
    }
    
    /// <summary>
    /// Получить пакет.
    /// </summary>
    /// <param name="packageService"></param>
    /// <param name="packageId">Идентификатор пакета.</param>
    /// <returns>Данные пакета зоны.</returns>
    [GraphQLName("GetPackage")]
    public async Task<Package> GetPackage([Service(ServiceKind.Resolver)] IPackageService packageService,
        Guid packageId)
    {
        var package = await packageService.GetPackageById(packageId);

        return PackageConverter.ConvertCoreToDtoModel(package);
    }
}