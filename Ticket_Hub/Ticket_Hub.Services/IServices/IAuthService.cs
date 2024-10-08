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
    Task<ResponseDto> CheckEmailExist(string email);
    Task<ResponseDto> UploadUserAvatar(IFormFile file, ClaimsPrincipal user);
    Task<MemoryStream> GetUserAvatar(ClaimsPrincipal user);
    Task<ResponseDto> SendVerifyEmail(string email, string confirmationLink);
    Task<ResponseDto> VerifyEmail(string userId, string token);
}