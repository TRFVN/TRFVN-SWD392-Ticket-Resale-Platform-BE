using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text;
using System.Web;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Auth;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Utility.Constants;

namespace Ticket_Hub.Services.Services;

public class AuthService : IAuthService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IFirebaseService _firebaseService;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private static readonly ConcurrentDictionary<string, (int Count, DateTime LastRequest)> ResetPasswordAttempts =
        new();

    public AuthService
    (
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IFirebaseService firebaseService,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _tokenService = tokenService;
        _firebaseService = firebaseService;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Sign up a new User.
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    public async Task<ResponseDto> SignUp(RegisterDto registerDto)
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
            Address = registerDto.Address,
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
        var isSuccess = await _unitOfWork.SaveAsync();
        return new ResponseDto()
        {
            Message = "User created successfully",
            IsSuccess = true,
            StatusCode = 201,
            Result = registerDto
        };
    }


    /// <summary>
    /// Sign in a User.
    /// </summary>
    /// <param name="signDto"></param>
    /// <returns></returns>
    public async Task<ResponseDto> SignIn(SignDto signDto)
    {
        var user = await _userManager.FindByEmailAsync(signDto.Email);
        if (user == null)
        {
            return new ResponseDto()
            {
                Message = "User does not exist!",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, signDto.Password);

        if (!isPasswordCorrect)
        {
            return new ResponseDto()
            {
                Message = "Incorrect email or password",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        if (!user.EmailConfirmed)
        {
            return new ResponseDto()
            {
                Message = "You need to confirm email!",
                Result = null,
                IsSuccess = false,
                StatusCode = 401
            };
        }

        if (user.LockoutEnd is not null)
        {
            return new ResponseDto()
            {
                Message = "User has been locked",
                IsSuccess = false,
                StatusCode = 403,
                Result = null
            };
        }


        var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
        var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
        //await _tokenService.StoreRefreshToken(user.Id, refreshToken);

        return new ResponseDto()
        {
            Result = new SignResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            },
            Message = "Sign in successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    /// <summary>
    /// Sign in a User by Google.
    /// </summary>
    /// <param name="signInByGoogleDto"></param>
    /// <returns></returns>
    public async Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto)
    {
        // Lấy thông tin từ Google
        FirebaseToken googleTokenS = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(signInByGoogleDto.Token);
        string userId = googleTokenS.Uid;
        string email = googleTokenS.Claims["email"].ToString();
        string name = googleTokenS.Claims["name"].ToString();
        string avatarUrl = googleTokenS.Claims["picture"].ToString();

        // Tìm kiếm người dùng trong database
        var user = await _userManager.FindByEmailAsync(email);
        UserLoginInfo? userLoginInfo = null;
        if (user is not null)
        {
            userLoginInfo = (await _userManager.GetLoginsAsync(user))
                .FirstOrDefault(x => x.LoginProvider == StaticLoginProvider.Google);
        }

        if (user?.LockoutEnd is not null)
        {
            return new ResponseDto()
            {
                Message = "User has been locked",
                IsSuccess = false,
                StatusCode = 403,
                Result = null
            };
        }

        if (user is not null && userLoginInfo is null)
        {
            return new ResponseDto()
            {
                Result = new SignResponseDto()
                {
                    RefreshToken = null,
                    AccessToken = null,
                },
                Message = "The email is using by another user",
                IsSuccess = false,
                StatusCode = 400
            };
        }

        if (userLoginInfo is null && user is null)
        {
            // Tạo user mới nếu chưa có trong database
            user = new ApplicationUser
            {
                Email = email,
                FullName = name,
                UserName = email,
                AvatarUrl = avatarUrl,
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user, new UserLoginInfo(StaticLoginProvider.Google, userId, "GOOGLE"));
        }

        var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
        var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
        await _tokenService.StoreRefreshToken(user.Id, refreshToken);
        await _userManager.UpdateAsync(user);

        return new ResponseDto()
        {
            Result = new SignResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            },
            Message = "Sign in successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public Task<ResponseDto> CheckEmailExist(string email)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Upload user avatar.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="User"></param>
    /// <returns></returns>
    public async Task<ResponseDto> UploadUserAvatar(IFormFile file, ClaimsPrincipal User)
    {
        // Lấy userId từ token
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        // Kiểm tra nếu người dùng chưa xác thực
        if (string.IsNullOrEmpty(userId))
        {
            return new ResponseDto()
            {
                Message = "Not authenticated!",
                Result = null,
                IsSuccess = false,
                StatusCode = 401 // Unauthorized
            };
        }

        // Tìm user theo userId
        var user = await _userManager.FindByIdAsync(userId);

        // Kiểm tra nếu user không tồn tại
        if (user == null)
        {
            return new ResponseDto()
            {
                Message = "User does not exist",
                Result = null,
                IsSuccess = false,
                StatusCode = 404 // Not Found
            };
        }

        // Gọi service Firebase để upload ảnh
        var responseDto = await _firebaseService.UploadImage(file, StaticFirebaseFolders.UserAvatars);

        // Kiểm tra kết quả upload
        if (!responseDto.IsSuccess)
        {
            return new ResponseDto()
            {
                Message = "Image upload failed!",
                Result = null,
                IsSuccess = false,
                StatusCode = 400 // Bad Request
            };
        }

        // Cập nhật AvatarUrl của user
        user.AvatarUrl = responseDto.Result?.ToString();
        var updateResult = await _userManager.UpdateAsync(user);

        // Kiểm tra kết quả cập nhật
        if (!updateResult.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Update user avatar failed!",
                Result = null,
                IsSuccess = false,
                StatusCode = 500 // Internal Server Error
            };
        }

        // Trả về kết quả thành công
        return new ResponseDto()
        {
            Message = "Upload user avatar successfully!",
            Result = null,
            IsSuccess = true,
            StatusCode = 200 // OK
        };
    }

    public async Task<MemoryStream> GetUserAvatar(ClaimsPrincipal User)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var user = await _userManager.FindByIdAsync(userId);

        var stream = await _firebaseService.GetImage(user.AvatarUrl);

        return stream;
    }

    public async Task<ResponseDto> SendVerifyEmail(string email, string confirmationLink)
    {
        await _emailService.SendVerifyEmail(email, confirmationLink);
        return new()
        {
            Message = "Send verify email successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = null
        };
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<ResponseDto> VerifyEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user.EmailConfirmed)
        {
            return new ResponseDto()
            {
                Message = "Your email has been confirmed!",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };
        }

        string decodedToken = HttpUtility.UrlDecode(token);

        var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!confirmResult.Succeeded)
        {
            return new()
            {
                Message = "Invalid token",
                StatusCode = 400,
                IsSuccess = false,
                Result = null
            };
        }

        return new()
        {
            Message = "Confirm Email Successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = null
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <param name="confirmNewPassword"></param>
    /// <returns></returns>
    public async Task<ResponseDto> ChangePassword(string userId, string oldPassword, string newPassword,
        string confirmNewPassword)
    {
        // Lấy id của người dùng
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ResponseDto { IsSuccess = false, Message = "User not found." };
        }

        // Thực hiện xác thực mật khẩu và thay đổi mật khẩu

        // Kiểm tra sự trùng khớp của mật khẩu mới và xác nhận mật khẩu mới 
        if (newPassword != confirmNewPassword)
        {
            return new ResponseDto
                { IsSuccess = false, Message = "New password and confirm new password not match." };
        }

        // Không cho phép thay đổi mật khẩu cũ
        if (newPassword == oldPassword)
        {
            return new ResponseDto
                { IsSuccess = false, Message = "New password cannot be the same as the old password." };
        }

        // Thực hiện thay đổi mật khẩu
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (result.Succeeded)
        {
            return new ResponseDto { IsSuccess = true, Message = "Password changed successfully." };
        }
        else
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Password change failed. Please ensure the old password is correct."
            };
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private string ip;

    private string city;
    private string region;
    private string country;
    private const int MaxAttemptsPerDay = 3;

    public async Task<ResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            // Tìm người dùng theo Email/Số điện thoại
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.EmailOrPhone);
            if (user == null)
            {
                user = await _userManager.Users.FirstOrDefaultAsync(
                    u => u.PhoneNumber == forgotPasswordDto.EmailOrPhone);
            }

            if (user == null || !user.EmailConfirmed)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "No user found or account not activated.",
                    StatusCode = 400
                };
            }

            // Kiểm tra giới hạn gửi yêu cầu đặt lại mật khẩu
            var email = user.Email;
            var now = DateTime.Now;

            if (ResetPasswordAttempts.TryGetValue(email, out var attempts))
            {
                // Kiểm tra xem đã quá 1 ngày kể từ lần thử cuối cùng chưa
                if (now - attempts.LastRequest >= TimeSpan.FromSeconds(1))
                {
                    // Reset số lần thử về 0 và cập nhật thời gian thử cuối cùng
                    ResetPasswordAttempts[email] = (1, now);
                }
                else if (attempts.Count >= MaxAttemptsPerDay)
                {
                    // Quá số lần reset cho phép trong vòng 1 ngày, gửi thông báo 
                    await _emailService.SendEmailAsync(user.Email,
                        "Password Reset Request Limit Exceeded",
                        $"You have exceeded the daily limit for password reset requests. Please try again after 24 hours."
                    );

                    // Vẫn trong thời gian chặn, trả về lỗi
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message =
                            "You have exceeded the daily limit for password reset requests. Please try again after 24 hours.",
                        StatusCode = 429
                    };
                }
                else
                {
                    // Chưa vượt quá số lần thử và thời gian chờ, tăng số lần thử và cập nhật thời gian
                    ResetPasswordAttempts[email] = (attempts.Count + 1, now);
                }
            }
            else
            {
                // Email chưa có trong danh sách, thêm mới với số lần thử là 1 và thời gian hiện tại
                ResetPasswordAttempts.AddOrUpdate(email, (1, now), (key, old) => (old.Count + 1, now));
            }

            // Tạo mã token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Gửi email chứa đường link đặt lại mật khẩu. //reset-password

            var resetLink = $"https://fpassword.w3spaces.com?token={token}&email={user.Email}";

            // Lấy ngày hiện tại
            var currentDate = DateTime.Now.ToString("MMMM d, yyyy");

            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"];

            // Lấy tên hệ điều hành
            var operatingSystem = GetUserAgentOperatingSystem(userAgent);

            // Lấy tên trình duyệt
            var browser = GetUserAgentBrowser(userAgent);

            // Lấy location
            var url = "https://ipinfo.io/14.169.10.115/json?token=823e5c403c980f";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(jsonContent);

                    this.ip = data["ip"].ToString();
                    this.city = data["city"].ToString();
                    this.region = data["region"].ToString();
                    this.country = data["country"].ToString();
                }
                else
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Error: Unable to retrieve data.",
                        StatusCode = 400
                    };
                }
            }

            // Gửi email chứa đường link đặt lại mật khẩu
            await _emailService.SendEmailResetAsync(user.Email, "Reset password for your Cursus account", user,
                currentDate, resetLink, operatingSystem, browser, ip, region, city, country);

            // Helper functions (you might need to refine these based on your User-Agent parsing logic)
            string GetUserAgentOperatingSystem(string userAgent)
            {
                // ... Logic to extract the operating system from the user-agent string
                // Example:
                if (userAgent.Contains("Windows")) return "Windows";
                else if (userAgent.Contains("Mac")) return "macOS";
                else if (userAgent.Contains("Linux")) return "Linux";
                else return "Unknown";
            }

            string GetUserAgentBrowser(string userAgent)
            {
                // ... Logic to extract the browser from the user-agent string
                // Example:
                if (userAgent.Contains("Chrome")) return "Chrome";
                else if (userAgent.Contains("Firefox")) return "Firefox";
                else if (userAgent.Contains("Safari")) return "Safari";
                else return "Unknown";
            }

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "The password reset link has been sent to your email.",
                StatusCode = 200
            };
        }
        catch (Exception e)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = e.Message,
                StatusCode = 500
            };
        }
    }
    
    public async Task<ResponseDto> ResetPassword(string email, string token, string password)
    {
        // Tìm người dùng theo email
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found.",
                StatusCode = 400
            };
        }

        // Kiểm tra xem mật khẩu mới có trùng với mật khẩu cũ hay không
        if (await _userManager.CheckPasswordAsync(user, password))
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "New password cannot be the same as the old password.",
                StatusCode = 400
            };
        }

        // Xác thực token và reset mật khẩu
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (result.Succeeded)
        {
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Reset password successfully.",
                StatusCode = 200
            };
        }
        else
        {
            // Xử lý lỗi nếu token không hợp lệ hoặc có lỗi khác
            StringBuilder errors = new StringBuilder();
            foreach (var error in result.Errors)
            {
                errors.AppendLine(error.Description);
            }

            return new ResponseDto
            {
                IsSuccess = false,
                Message = errors.ToString(),
                StatusCode = 400
            };
        }
    }
}