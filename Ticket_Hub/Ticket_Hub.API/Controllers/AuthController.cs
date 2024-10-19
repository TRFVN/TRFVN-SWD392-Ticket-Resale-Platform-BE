using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Sign in a User.
        /// </summary>
        /// <param name="signDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult<ResponseDto>> SignIn([FromBody] SignDto signDto)
        {
            var responseDto = await _authService.SignIn(signDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        /// <summary>
        /// Sign in a User by Google.
        /// </summary>
        /// <param name="signInByGoogleDto"></param>
        /// <returns></returns>
        [HttpPost("sign-in-google")]
        public async Task<ActionResult<ResponseDto>> SignInByGoogle([FromBody] SignInByGoogleDto signInByGoogleDto)
        {
            var responseDto = await _authService.SignInByGoogle(signInByGoogleDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("FetchUserByToken")]
        public async Task<IActionResult> FetchUserByToken(string token)
        {
            var result = await _authService.FetchUserByToken(token);
            var responseDto = await _authService.FetchUserByToken(token);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarUploadDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/avatar")]
        //[Authorize]
        public async Task<ActionResult<ResponseDto>> UploadUserAvatar(AvatarUploadDto avatarUploadDto)
        {
            var response = await _authService.UploadUserAvatar(avatarUploadDto.File, User);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get User Avatar.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user/avatar")]
        //[Authorize]
        public async Task<IActionResult> GetUserAvatar()
        {
            var stream = await _authService.GetUserAvatar(User);
            if (stream is null)
            {
                return NotFound("User avatar does not exist!");
            }

            return File(stream, "image/png");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send-verify-email")]
        public async Task<ActionResult<ResponseDto>> SendVerifyEmail([FromBody] SendVerifyEmailDto email)
        {
            var user = await _userManager.FindByEmailAsync(email.Email);
            if (user.EmailConfirmed)
            {
                return new ResponseDto()
                {
                    IsSuccess = true,
                    Message = "Your email has been confirmed",
                    StatusCode = 200,
                    Result = email
                };
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink =
                $"{Request.Scheme}://{Request.Host}/user/sign-in/verify-email?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(token)}";

            var responseDto = await _authService.SendVerifyEmail(user.Email, confirmationLink);

            return StatusCode(responseDto.StatusCode, responseDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("verify-email")]
        [ActionName("verify-email")]
        public async Task<ActionResult<ResponseDto>> VerifyEmail(
            [FromQuery] string userId,
            [FromQuery] string token)
        {
            var responseDto = await _authService.VerifyEmail(userId, token);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-password")]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            // Lấy Id người dùng hiện tại.
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _authService.ChangePassword(userId, changePasswordDto.OldPassword,
                changePasswordDto.NewPassword, changePasswordDto.ConfirmNewPassword);

            if (response.IsSuccess)
            {
                return Ok(response.Message);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpGet]
        [Route("ForgotPassword")]
        public async Task<ActionResult<ResponseDto>> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Email is not exist",
                    StatusCode = 404
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var confirmationLink =
                $"{Request.Scheme}://{Request.Host}/user/sign-in/reset-password?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(token)}";

            var responseDto = await _authService.SendVerifyEmail(user.Email, confirmationLink);

            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<ActionResult<ResponseDto>> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var result = await _authService.ForgotPassword(forgotPasswordDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ResponseDto>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await _authService.ResetPassword(resetPasswordDto.Email, resetPasswordDto.Token,
                resetPasswordDto.Password);
            return StatusCode(result.StatusCode, result);
        }
    }
}