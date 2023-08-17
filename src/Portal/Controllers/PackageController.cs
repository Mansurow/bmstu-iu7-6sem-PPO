using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Models;
using Portal.Common.Models.Dto;
using Portal.Common.Models.Enums;
using Portal.Services.PackageService;
using Portal.Services.PackageService.Exceptions;

namespace Portal.Controllers;

/// <summary>
/// Контроллер пакетов
/// </summary>
[ApiController]
[Route("api/v1/packages")]
public class PackageController: ControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ILogger<PackageController> _logger;

    /// <summary>
    /// Конструктор контроллера пакета
    /// </summary>
    /// <param name="packageService">Сервис пакетов</param>
    /// <param name="logger">Инструмент логгирования</param>
    /// <exception cref="ArgumentNullException">Ошибка происходит, если парметры переданы неверно</exception>
    public PackageController(IPackageService packageService, ILogger<PackageController> logger)
    {
        _packageService = packageService ?? throw new ArgumentNullException(nameof(packageService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить все пакеты
    /// </summary>
    /// <returns>Возвращается список пакетов</returns>
    /// <response code="200">OK. Возвращается список пакетов.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
    
    /// <summary>
    /// Получить пакет
    /// </summary>
    /// <param name="packageId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны</param>
    /// <returns>Возвращается пакет</returns>
    /// <response code="200">OK. Возвращается пакет.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="404">NotFound. Пакет не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("{packageId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Package))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
    
    /// <summary>
    /// Добавить пакет
    /// </summary>
    /// <param name="package">Данные о пакете</param>
    /// <returns>Идентификатор пакета</returns>
    /// <response code="200">Ok. Идентификатор пакета.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost]
    [Authorize(Roles = nameof(Role.Administrator))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostPackage([FromBody] CreatePackageDto package)
    {
        try
        {
            var packageId = await _packageService.AddPackageAsync(package.Name, package.Type, package.Price, 
                package.RentalTime, package.Description, package.Dishes);

            return Ok(new { packageId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // TODO: добавить обновление пакета 
    
    /// <summary>
    /// Удалить пакет
    /// </summary>
    /// <param name="packageId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор пакета</param>
    /// <response code="204">NoContent. Пакет успешно обновлена.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Пакет не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpDelete("{packageId:guid}")]
    [Authorize(Roles = nameof(Role.Administrator))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
}