using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Utility.Constants;

namespace Ticket_Hub.Services.Services;

public class AuthService : IAuthService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly ITokenService _tokenService;
    //private readonly IFirebaseService _firebaseService;
    //private readonly IEmailService _emailService;

    private static readonly ConcurrentDictionary<string, (int Count, DateTime LastRequest)> ResetPasswordAttempts =
        new();

    public AuthService
    (
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager
        //ITokenService tokenService,
        //IFirebaseService firebaseService,
        //IEmailService emailService
    )
    {
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        //_tokenService = tokenService;
        //_firebaseService = firebaseService;
        //_emailService = emailService;
    }

    public async Task<ResponseDto> SignUp(RegisterDto registerDto)
    {
        try
        {

            // Kiểm tra email đã tồn tại
            var isEmailExit = await _userManager.FindByEmailAsync(registerDto.Email);
            if (isEmailExit is not null)
            {
                return new ResponseDto()
                {
                    Message = "Email is being used by another user",
                    Result = registerDto,
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            // Kiểm tra số điện thoại đã tồn tại
            var isPhoneNumberExit = await _userManager.Users
                .AnyAsync(u => u.PhoneNumber == registerDto.PhoneNumber);
            if (isPhoneNumberExit)
            {
                return new ResponseDto()
                {
                    Message = "Phone number is being used by another user",
                    Result = registerDto,
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            // Tạo đối tượng ApplicationUser mới
            ApplicationUser newUser = new ApplicationUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FullName = registerDto.FullName,
                Country = registerDto.Country,
                BirthDate = registerDto.BirthDate,
                PhoneNumber = registerDto.PhoneNumber,
                Cccd = registerDto.Cccd,
                AvatarUrl = "",
                LockoutEnabled = false
            };

            // Thêm người dùng mới vào database
            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            // Kiểm tra lỗi khi tạo
            if (!createUserResult.Succeeded)
            {
                return new ResponseDto()
                {
                    Message = "Create user failed",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = registerDto
                };
            }

            var user = newUser;
            var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Member);

            if (!isRoleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Member));
            }

            // Thêm role "Customer" cho người dùng
            var isRoleAdded = await _userManager.AddToRoleAsync(user, StaticUserRoles.Member);

            if (!isRoleAdded.Succeeded)
            {
                return new ResponseDto()
                {
                    Message = "Error adding role",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = registerDto
                };
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.SaveAsync();
            return new ResponseDto()
            {
                Message = "User created successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = registerDto
            };
        }catch (Exception e)
        {
            return new ResponseDto()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = registerDto
            };
        }
    }

    public Task<ResponseDto> SignIn(SignDto signDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> CheckEmailExist(string email)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UploadUserAvatar(IFormFile file, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<MemoryStream> GetUserAvatar(ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> SendVerifyEmail(string email, string confirmationLink)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> VerifyEmail(string userId, string token)
    {
        throw new NotImplementedException();
    }
}