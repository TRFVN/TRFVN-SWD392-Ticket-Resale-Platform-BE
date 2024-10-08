using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Auth;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public AuthController(UserManager<ApplicationUser> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        /// <summary>
        /// Register a new User.
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost("users")]
        public async Task<ActionResult<ResponseDto>> SignUp([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var result = await _authService.SignUp(registerDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message
                });
            }
        }
        
        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult<ResponseDto>> SignIn([FromBody] SignDto signDto)
        {
            var responseDto = await _authService.SignIn(signDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
