using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.RoomService;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.Repositories;
using Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Anticafe.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        private readonly ILogger<RoomController> _logger;

        public RoomController(IRoomService roomService, ILogger<RoomController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomDto[]))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsAsync();
                _logger.LogInformation("Get all rooms information successfully.");
                return Ok(rooms.Select(RoomConverter.ConvertAppModelToDto).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetRoomInfo([FromRoute] int roomId)
        {
            try
            {
                var rooms = await _roomService.GetRoomByIdAsync(roomId);
                _logger.LogInformation("Get room information successfully.");
                return Ok(RoomConverter.ConvertAppModelToDto(rooms));
            }
            catch (RoomNotFoundException ex)
            {
                _logger.LogError(ex, "{ex.Message}", ex.Message);
                return NotFound(ex.Message);
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
        public async Task<IActionResult> AddRoom([FromBody] RoomDto room)
        {
            try
            {
                await _roomService.AddRoomAsync(room.Name, room.Size, room.Price);
                _logger.LogInformation("Room add successfully.");
                return Ok();
            }
            catch (RoomNameExistException ex)
            {
                _logger.LogError(ex, "Room name exist: {ex.Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> UpdateRoom([FromRoute] int roomId, [FromBody] RoomDto roomDto)
        {
            try
            {
                var room = RoomConverter.ConvertDtoToAppModel(roomDto);
                await _roomService.UpdateRoomAsync(room);
                _logger.LogInformation("Room successfully update.");
                return Ok();
            }
            catch (RoomNotFoundException ex)
            {
                _logger.LogError(ex, "Room not exist: {ex.Message}", ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> DeleteRoom([FromRoute] int roomId)
        {
            try
            {
                await _roomService.DeleteRoomAsync(roomId);
                _logger.LogInformation("Room successfully delete.");
                return Ok();
            }
            catch (RoomNotFoundException ex)
            {
                _logger.LogError(ex, "Room not exist: {ex.Message}", ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpPut("{roomId}/inventory/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> AddInventoryRoom([FromRoute] int roomId, [FromRoute] string name)
        {
            try
            {
                var inventory = new Inventory(0, name);
                await _roomService.AddInventoryForRoomAsync(roomId, inventory);
                _logger.LogInformation("Inventory add for room successfully.");
                return Ok();
            }
            catch (RoomNotFoundException ex)
            {
                _logger.LogError(ex, "Room not exist: {ex.Message}", ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
