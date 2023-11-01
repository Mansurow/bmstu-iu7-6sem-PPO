using HotChocolate.Authorization;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.Package;
using Portal.Common.Enums;
using Portal.Services.PackageService;

namespace Portal.Graphql.Mutations;

[ExtendObjectType("Mutation")]
public class PackageMutation
{
    /// <summary>
    /// Добавить пакет.
    /// </summary>
    /// <param name="packageService"></param>
    /// <param name="createPackage">Данные для добваления пакета.</param>
    /// <returns>Идентификатор пакета.</returns>
    [GraphQLName("AddPackage")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> AddPackage([Service(ServiceKind.Resolver)] IPackageService packageService, 
        CreatePackage createPackage)
    {
        var packageId = await packageService.AddPackageAsync(
            createPackage.Name, 
            createPackage.Type, 
            createPackage.Price, 
            createPackage.RentalTime, 
            createPackage.Description, 
            createPackage.Dishes);
            
        return new IdResponse(packageId);
    }
    
    /// <summary>
    /// Обновить пакет. 
    /// </summary>
    /// <param name="packageService"></param>
    /// <param name="updatePackage">Данные для обновления пакета.</param>
    [GraphQLName("UpdatePackage")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> UpdatePackage([Service(ServiceKind.Resolver)] IPackageService packageService, 
        Package updatePackage)
    {
        var package = PackageConverter.ConvertDtoToCoreModel(updatePackage);
            
        await packageService.UpdatePackageAsync(package);
        
        return new IdResponse(updatePackage.Id);
    }
    
    /// <summary>
    /// Удалить пакет.
    /// </summary>
    /// <param name="packageService"></param>
    /// <param name="packageId">Идентификатор пакета.</param>
    [GraphQLName("DeletePackage")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> DeletePackage([Service(ServiceKind.Resolver)] IPackageService packageService,
        Guid packageId)
    {
        await packageService.RemovePackageAsync(packageId);

        return new IdResponse(packageId);
    }
}