using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Models;
using Portal.Common.Models.Dto;
using Portal.Common.Models.Enums;
using Portal.Services.FeedbackService;
using Portal.Services.FeedbackService.Exceptions;
using Portal.Services.UserService.Exceptions;
using Portal.Services.ZoneService.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Portal.Controllers;

/// <summary>
/// Контроллер отзывов
/// </summary>
[ApiController]
[Route("api/v1/feedbacks/")]
public class FeedbackController: ControllerBase
{
    private readonly IFeedbackService _feedbackService;
    private readonly ILogger<FeedbackController> _logger;

    /// <summary>
    /// Конструктор контроллера отзывов
    /// </summary>
    /// <param name="feedbackService">Сервис отзывов</param>
    /// <param name="logger">Инструмент логгирования</param>
    /// <exception cref="ArgumentNullException">Ошибка происходит, если парметры переданы неверно</exception>
    public FeedbackController(IFeedbackService feedbackService, ILogger<FeedbackController> logger)
    {
        _feedbackService = feedbackService ?? throw new ArgumentNullException(nameof(feedbackService));
        _logger = logger  ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить все отзывы пользователей
    /// </summary>
    /// <returns>Возвращается список всех отзывов</returns>
    /// <response code="200">Bad request. Возвращается список всех отзывов.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    [Authorize(Roles = nameof(Role.Administrator))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Feedback>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeedbacks()
    {
        try
        {
            var feedbacks = await _feedbackService.GetAllFeedbackAsync();

            return Ok(feedbacks);
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
    /// Получить все отзывы пользователей для комнаты
    /// </summary>
    /// <param name="zoneId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны</param>
    /// <returns>Возвращается список отзывов комнаты</returns>
    /// <response code="200">Bad request. Возвращается список отзывов.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="404">NotFound. Зона не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("{zoneId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Feedback>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetZoneFeedbacks(Guid zoneId)
    {
        try
        {
            var feedbacks = await _feedbackService.GetAllFeedbackByZoneAsync(zoneId);

            return Ok(feedbacks);
        }
        catch (ZoneNotFoundException e)
        {
            _logger.LogError("Zone: {ZoneId} not found", zoneId);
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
    /// Добавить отзыв
    /// </summary>
    /// <param name="feedbackDto">Данные для добавления отзыва</param>
    /// <returns>Идентификатор добавленного отзыва</returns>
    /// <response code="200">Bad request. Идентификатор отзыв.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost]
    [Authorize(Roles = nameof(Role.User))]
    [SwaggerResponse(statusCode: 200, description: "Идентификатор отзыв.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PostFeedback([FromBody] CreateFeedbackDto feedbackDto)
    {
        try
        {
            var feedbackId = await _feedbackService.AddFeedbackAsync(feedbackDto.ZoneId, feedbackDto.UserId,
                feedbackDto.Mark, feedbackDto.Message);

            return Ok(new { feedbackId });
        }
        catch (UserNotFoundException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest,new
            {
                messsage = e.Message
            });
        }
        catch (ZoneNotFoundException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest,new
            {
                messsage = e.Message
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
    /// Удалить отзыв
    /// </summary>
    /// <param name="feedbackId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор отзыва</param>
    /// <returns>Идентификатор добавленного отзыва</returns>
    /// <response code="200">Bad request. Идентификатор отзыв.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Отзыв не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpDelete("{feedbackId:guid}")]
    [Authorize(Roles = nameof(Role.User))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFeedback([FromRoute] Guid feedbackId)
    {
        try
        {
            await _feedbackService.RemoveFeedbackAsync(feedbackId);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (FeedbackNotFoundException e)
        {
            _logger.LogError(e, "Feedback: {FeedbackId} not found", feedbackId);
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