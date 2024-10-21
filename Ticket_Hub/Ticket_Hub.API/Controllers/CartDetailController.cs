using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.DTO.CartHeader;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Services.Services;
using Ticket_Hub.Models.DTO.CartDetail;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailController : ControllerBase
    {
        private readonly ICartDetailService _cartDetailService;

        public CartDetailController(ICartDetailService cartDetailService)
        {
            _cartDetailService = cartDetailService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetCartDetails
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
        )
        {
            var responseDto =
                await _cartDetailService.GetCartDetails(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{cartDetailId}")]
        public async Task<ActionResult<ResponseDto>> GetCartDetail
        (
            [FromRoute] Guid cartDetailId
        )
        {
            var responseDto = await _cartDetailService.GetCartDetail(User, cartDetailId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateCartDetail
        (
            [FromBody] CreateCartDetailDto createCartDetailDto
        )
        {
            var responseDto = await _cartDetailService.CreateCartDetail(User, createCartDetailDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateCartDetail
        (
            [FromBody] UpdateCartDetailDto updateCartDetailDto
        )
        {
            var responseDto = await _cartDetailService.UpdateCartDetail(User, updateCartDetailDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{cartDetailId}")]
        public async Task<ActionResult<ResponseDto>> DeleteCartDetail
        (
            [FromRoute] Guid cartDetailId
        )
        {
            var responseDto = await _cartDetailService.DeleteCartDetail(User, cartDetailId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
