using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.CartHeader;
using Ticket_Hub.Models.DTO.Category;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Services.Services;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartHeaderController : ControllerBase
    {
        private readonly ICartHeaderService _cartHeaderService;

        public CartHeaderController(ICartHeaderService cartHeaderService)
        {
            _cartHeaderService = cartHeaderService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetCartHeaders
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var responseDto =
                await _cartHeaderService.GetCartHeaders(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{cartHeaderId}")]
        public async Task<ActionResult<ResponseDto>> GetCartHeader
        (
            [FromRoute] Guid cartHeaderId
        )
        {
            var responseDto = await _cartHeaderService.GetCartHeader(User, cartHeaderId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateCartHeader
        (
            [FromBody] CreateCartHeaderDto createCartHeaderDto
        )
        {
            var responseDto = await _cartHeaderService.CreateCartHeader(User, createCartHeaderDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateCartHeader
        (
            [FromBody] UpdateCartHeaderDto updateCartHeaderDto
        )
        {
            var responseDto = await _cartHeaderService.UpdateCartHeader(User, updateCartHeaderDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{cartHeaderId}")]
        public async Task<ActionResult<ResponseDto>> DeleteCartHeader
        (
            [FromRoute] Guid cartHeaderId
        )
        {
            var responseDto = await _cartHeaderService.DeleteCartHeader(User, cartHeaderId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
