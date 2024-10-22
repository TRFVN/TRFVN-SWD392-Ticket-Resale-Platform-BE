using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Favourite;
using Ticket_Hub.Models.DTO.Feedback;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Services.Services;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetFavourites
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 0
        )
        {
            var responseDto = await _favouriteService.GetFavourites(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{favouriteId}")]
        public async Task<ActionResult<ResponseDto>> GetFavourite
        (
            [FromRoute] Guid favouriteId
        )
        {
            var responseDto = await _favouriteService.GetFavourite(User, favouriteId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateFavourite
        (
            [FromBody] CreateFavouriteDto createFavouriteDto
        )
        {
            var responseDto = await _favouriteService.CreateFavourite(User, createFavouriteDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateFavourite
        (
            [FromBody] UpdateFavouriteDto updateFavouriteDto
        )
        {
            var responseDto = await _favouriteService.UpdateFavourite(User, updateFavouriteDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{favouriteId}")]
        public async Task<ActionResult<ResponseDto>> DeleteFavourite
        (
            [FromRoute] Guid favouriteId
        )
        {
            var responseDto = await _favouriteService.DeleteFavourite(User, favouriteId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
