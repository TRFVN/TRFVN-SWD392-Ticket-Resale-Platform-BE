using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Auth;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Utility.Constants;

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
            var responseDto = await _authService.UploadUserAvatar(avatarUploadDto.File, User);
            return StatusCode(responseDto.StatusCode, responseDto);
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

            var responseDto = await _authService.ChangePassword(userId, changePasswordDto.OldPassword,
                changePasswordDto.NewPassword, changePasswordDto.ConfirmNewPassword);

            if (responseDto.IsSuccess)
            {
                return Ok(responseDto.Message);
            }
            else
            {
                return BadRequest(responseDto.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forgotPasswordDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgot-password")]
        public async Task<ActionResult<ResponseDto>> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var responseDto = await _authService.ForgotPassword(forgotPasswordDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetPasswordDto"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<ActionResult<ResponseDto>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var responseDto = await _authService.ResetPassword(resetPasswordDto.Email, resetPasswordDto.Token,
                resetPasswordDto.Password);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Lock User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("lock-user")]
        [Authorize(Roles = StaticUserRoles.Admin)]
        public async Task<ActionResult<ResponseDto>> LockUser(string id)
        {
            var responseDto = await _authService.LockUser(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Unlock User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("unlock-user")]
        [Authorize(Roles = StaticUserRoles.Admin)]
        public async Task<ActionResult<ResponseDto>> UnlockUser(string id)
        {
            var responseDto = await _authService.UnlockUser(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet]
        [Authorize(Roles = StaticUserRoles.Admin)]
        public async Task<ActionResult<ResponseDto>> GetAllUsers(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var responseDto = await _authService.GetAllUsers(filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}