using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Microsoft.AspNetCore.Http;
using Ticket_Hub.Models.DTO.Auth;

namespace Ticket_Hub.Services.IServices;

public interface IAuthService
{
    Task<ResponseDto> SignUp(RegisterDto registerDto);
    Task<ResponseDto> SignIn(SignDto signDto);
    Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto);
    Task<ResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto);
    Task<ResponseDto> FetchUserByToken(string token);
    Task<ResponseDto> UploadUserAvatar(IFormFile file, ClaimsPrincipal user);
    Task<MemoryStream> GetUserAvatar(ClaimsPrincipal user);
    Task<ResponseDto> SendVerifyEmail(string email, string confirmationLink);
    Task<ResponseDto> VerifyEmail(string userId, string token);
    Task<ResponseDto> ChangePassword(string userId, string oldPassword, string newPassword, string confirmNewPassword);
    Task<ResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    Task<ResponseDto> ResetPassword(string resetPasswordDto, string token, string password);
    Task<ResponseDto> LockUser(string id);
    Task<ResponseDto> UnlockUser(string id);
    Task<ResponseDto> GetAllUsers(
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 10
    );
}