using Anticafe.BL.Models;
using Anticafe.BL.Services.StatisticsService;
using Anticafe.BL.Sevices.RoomService;
using Anticafe.Common.Models.DTO;
using Anticafe.Converter;
using Anticafe.DataAccess.Converter;
using Microsoft.AspNetCore.Mvc;

namespace Anticafe.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        private readonly ILogger<RoomController> _logger;

        public StatisticsController(IStatisticsService statisticsService, ILogger<RoomController> logger)
        {
            _statisticsService = statisticsService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingStatisticsDto[]))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var statistics = await _statisticsService.GetBookingStatisticsAsync();
                _logger.LogInformation("Get all bookings statistics information successfully.");
                return Ok(statistics.Select(StatisticsConverter.ConvertAppModelToDto).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("/{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingStatisticsDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetRoomStatistics([FromRoute] int roomId)
        {
            try
            {
                var statistics = await _statisticsService.GetBookingStatisticsByRoomAsync(roomId);
                _logger.LogInformation("Get all bookings statistics information successfully.");
                return Ok(StatisticsConverter.ConvertAppModelToDto(statistics));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
