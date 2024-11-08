using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO.Cart;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Utility.Constants;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = StaticUserRoles.Member)]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO addToCartDto)
        {
            var responseDto = await _cartService.AddToCart
            (
                User,
                addToCartDto
            );
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
