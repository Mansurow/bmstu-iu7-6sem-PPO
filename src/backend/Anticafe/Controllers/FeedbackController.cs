using Anticafe.BL.Sevices.FeedbackService;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.Repositories;
using Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Anticafe.Controllers;

[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    private readonly ILogger<FeedbackController> _logger;

    public FeedbackController(IFeedbackService feedbackService, ILogger<FeedbackController> logger)
    {
        _feedbackService = feedbackService;
        _logger = logger;
    }

    [HttpGet("{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedbackDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetAllRoomFeedbacks([FromRoute] int roomId)
    {
        try
        {
            var feedbacks = await _feedbackService.GetAllFeedbackByRoomAsync(roomId);
            _logger.LogInformation("Get user information successfully.");
            return Ok(feedbacks.Select(f => FeedbackConverter.ConvertAppModelToDto(f)).ToList());
        }
        catch (RoomNotFoundException ex)
        {
            _logger.LogError(ex, "Room not exist: {roomId}", roomId);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> AddFeedback([FromBody] FeedbackDto feedback)
    {
        try
        {
            await _feedbackService.AddFeedbackAsync(feedback.UserId, feedback.RoomId, feedback.Mark, feedback.Message);
            _logger.LogInformation("Feedback add successfully.");
            return Ok();
        }
        catch (RoomNotFoundException ex)
        {
            _logger.LogError(ex, "Room not found: {ex.Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundByIdException ex)
        {
            _logger.LogError(ex, "User not found: {ex.Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{feedbackId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeleteRoom([FromRoute] int feedbackId)
    {
        try
        {
            await _feedbackService.DeleteFeedbackAsync(feedbackId);
            _logger.LogInformation("Feedback successfully delete.");
            return Ok();
        }
        catch (FeedbackNotFoundException ex)
        {
            _logger.LogError(ex, "Feedback not exist: {ex.Message}", ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
