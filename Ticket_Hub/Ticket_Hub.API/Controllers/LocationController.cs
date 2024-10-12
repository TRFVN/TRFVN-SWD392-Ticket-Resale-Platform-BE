using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Location;
using Ticket_Hub.Services.IServices;


namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        
        /// <summary>
        /// Get all locations
        /// </summary>
        /// <param name="filterOn"></param>
        /// <param name="filterQuery"></param>
        /// <param name="sortBy"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetLocations
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var responseDto = await _locationService.GetLocations(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Get location by locationId
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpGet("{locationId}")]
        public async Task<ActionResult<ResponseDto>> GetLocation
        (
            [FromRoute] Guid locationId
        )
        {
            var responseDto = await _locationService.GetLocation(User, locationId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Create location
        /// </summary>
        /// <param name="createLocationDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> CreateLocation
        (
            [FromBody] CreateLocationDto createLocationDto
        )
        {
            var responseDto = await _locationService.CreateLocation(User, createLocationDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Update location
        /// </summary>
        /// <param name="updateLocationDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> UpdateLocation
        (
            [FromBody] UpdateLocationDto updateLocationDto
        )
        {
            var responseDto = await _locationService.UpdateLocation(User, updateLocationDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Delete location
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpDelete("{locationId}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> DeleteLocation
        (
            [FromRoute] Guid locationId
        )
        {
            var responseDto = await _locationService.DeleteLocation(User, locationId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}