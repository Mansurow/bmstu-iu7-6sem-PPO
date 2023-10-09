using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.Package;
using Portal.Common.Enums;
using Portal.Common.Models.Dto;
using Portal.Services.PackageService;
using Portal.Services.PackageService.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Portal.Controllers.v1;

/// <summary>
/// Контроллер пакетов.
/// </summary>
[ApiController]
[Route("api/v1/packages")]
public class PackageController: ControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ILogger<PackageController> _logger;

    /// <summary>
    /// Конструктор контроллера пакета.
    /// </summary>
    /// <param name="packageService">Сервис пакетов.</param>
    /// <param name="logger">Инструмент логгирования.</param>
    public PackageController(IPackageService packageService, ILogger<PackageController> logger)
    {
        _packageService = packageService;
        _logger = logger;
    }

    /// <summary>
    /// Получить все пакеты.
    /// </summary>
    /// <returns>Возвращается список пакетов.</returns>
    /// <response code="200">OK. Возвращается список пакетов.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Package>), description: "Возвращается список пакетов.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetPackages()
    {
        try
        {
            var packages = await _packageService.GetPackagesAsync();

            return Ok(packages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Получить пакет.
    /// </summary>
    /// <param name="packageId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны.</param>
    /// <returns>Возвращается пакет.</returns>
    /// <response code="200">OK. Возвращается пакет.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="404">NotFound. Пакет не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("{packageId:guid}")]
    [SwaggerResponse(statusCode: 200, type: typeof(Package), description: "Возвращается пакет.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetPackage([FromRoute] Guid packageId)
    {
        try
        {
            var package = await _packageService.GetPackageById(packageId);

            return Ok(package);
        }
        catch (PackageNotFoundException e)
        {
            _logger.LogError(e, "Package: {PackageId} not found", packageId);
            return NotFound(new
            {
                message = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Добавить пакет.
    /// </summary>
    /// <param name="package">Данные о пакете.</param>
    /// <returns>Идентификатор пакета.</returns>
    /// <response code="200">Ok. Идентификатор пакета.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 200, type: typeof(IdResponse), description: "Идентификатор пакета.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PostPackage([FromBody] CreatePackage package)
    {
        try
        {
            var packageId = await _packageService.AddPackageAsync(package.Name, package.Type, package.Price, 
                package.RentalTime, package.Description, package.Dishes);

            return Ok(new IdResponse() { Id = packageId });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }

    /// <summary>
    /// Обновить пакет.
    /// </summary>
    /// <param name="packageDto">Данные о пакете.</param>
    /// <response code="204">NotContent. Пакет успешно обновлен.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPut]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 204, description: "Пакет успешно обновлен.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PutPackage([FromBody] Package packageDto)
    {
        try
        {
            var package = PackageConverter.ConvertDtoToCoreModel(packageDto);
            
            await _packageService.UpdatePackageAsync(package);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (PackageNotFoundException e)
        {
            _logger.LogError(e, "Package {PackageId} not found", packageDto.Id);
            return NotFound(new
            {
                message = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }

    /// <summary>
    /// Удалить пакет.
    /// </summary>
    /// <param name="packageId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор пакета.</param>
    /// <response code="204">NoContent. Пакет успешно удален.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Пакет не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpDelete("{packageId:guid}")]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 204, description: "Пакет успешно удален.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Пакет не найдена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PostPackage([FromRoute] Guid packageId)
    {
        try
        {
            await _packageService.RemovePackageAsync(packageId);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (PackageNotFoundException e)
        {
            _logger.LogError(e, "Package: {PackageId} not found", packageId);
            return NotFound(new
            {
                message = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
}