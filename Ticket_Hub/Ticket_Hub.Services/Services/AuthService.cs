using System.Collections.Concurrent;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    private readonly JwtSecurityTokenHandler _tokenHandler;

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
        _tokenHandler = new JwtSecurityTokenHandler();
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
        await _unitOfWork.SaveAsync();
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

        // Xóa refresh token cũ nếu có
        var existingRefreshToken = await _tokenService.RetrieveRefreshToken(user.Id);
        if (existingRefreshToken != null)
        {
            await _tokenService.DeleteRefreshToken(user.Id);
        }

        //Tạo access token và refresh token mới
        var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
        var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
        await _tokenService.StoreRefreshToken(user.Id, refreshToken);

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
        // Gọi API của Google để lấy thông tin từ Access Token
        var httpClient = new HttpClient();
        var response =
            await httpClient.GetStringAsync(
                $"https://www.googleapis.com/oauth2/v3/tokeninfo?access_token={signInByGoogleDto.Token}");

        // Parse response từ Google
        var googleUser = JsonConvert.DeserializeObject<GoogleUserInfo>(response);
        if (googleUser == null || googleUser.email == null)
        {
            return new ResponseDto()
            {
                Message = "Invalid Google Access Token",
                IsSuccess = false,
                StatusCode = 401
            };
        }

        string email = googleUser.email;

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
                    RefreshToken = "",
                    AccessToken = "",
                },
                Message = "The email is using by another user",
                IsSuccess = false,
                StatusCode = 400
            };
        }

        // Nếu user chưa tồn tại, tạo user mới và thêm role "Member"
        if (user is null)
        {
            user = new ApplicationUser
            {
                Email = email,
                FullName = "",
                UserName = email,
                AvatarUrl = "",
                Country = "",
                Cccd = "",
                Address = "",
                EmailConfirmed = true
            };

            // Tạo user mới trong database
            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                return new ResponseDto()
                {
                    Message = "Error creating user",
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            // Thêm thông tin đăng nhập Google vào tài khoản
            await _userManager.AddLoginAsync(user, new UserLoginInfo(StaticLoginProvider.Google, googleUser.sub, "GOOGLE"));

            // Kiểm tra và tạo role "Member" nếu chưa có
            var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Member);
            if (!isRoleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Member));
            }

            // Thêm role "Member" cho người dùng mới
            var isRoleAdded = await _userManager.AddToRoleAsync(user, StaticUserRoles.Member);
            if (!isRoleAdded.Succeeded)
            {
                return new ResponseDto()
                {
                    Message = "Error adding role",
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
        }

        // Cập nhật thông tin người dùng
        await _userManager.UpdateAsync(user);

        // Kiểm tra thông tin bắt buộc đã được cập nhật chưa
        bool isProfileComplete =
            !string.IsNullOrEmpty(user.FullName) &&
            !string.IsNullOrEmpty(user.Address) &&
            !string.IsNullOrEmpty(user.AvatarUrl) &&
            !string.IsNullOrEmpty(user.Country) &&
            !string.IsNullOrEmpty(user.Cccd);

        // Tạo Access Token và Refresh Token cho user
        var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user!);
        var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user!);
        await _tokenService.StoreRefreshToken(user!.Id, refreshToken);

        // Nếu hồ sơ chưa hoàn chỉnh, trả về cảnh báo
        if (!isProfileComplete)
        {
            return new ResponseDto()
            {
                Result = new SignByGoogleResponseDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    IsProfileComplete = false // Thông báo nếu cần cập nhật thông tin
                },
                Message = "Your profile is incomplete. Please update your profile information.",
                IsSuccess = true, // Vẫn trả về thành công để cấp quyền truy cập
                StatusCode = 200
            };
        }

        // Nếu thông tin đầy đủ
        return new ResponseDto()
        {
            Result = new SignByGoogleResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IsProfileComplete = true
            },
            Message = "Sign in successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="updateUserProfileDto"></param>
    /// <returns></returns>
    public async Task<ResponseDto> UpdateUserProfile(string userId, UpdateUserProfileDto updateUserProfileDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };
        }

        // Cập nhật các trường thông tin
        user.FullName = updateUserProfileDto.FullName;
        user.AvatarUrl = updateUserProfileDto.AvatarUrl;
        user.Country = updateUserProfileDto.Country;
        user.Cccd = updateUserProfileDto.Cccd;
        user.Address = updateUserProfileDto.Address;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            return new ResponseDto
            {
                Message = "Failed to update user profile",
                IsSuccess = false,
                StatusCode = 400,
                Result = updateResult.Errors
            };
        }


        // Xóa refresh token cũ nếu có
        var existingRefreshToken = await _tokenService.RetrieveRefreshToken(user.Id);
        if (existingRefreshToken != null)
        {
            await _tokenService.DeleteRefreshToken(user.Id);
        }
        // Tạo Access Token và Refresh Token cho user
        var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user!);
        var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user!);
        await _tokenService.StoreRefreshToken(user!.Id, refreshToken);

        return new ResponseDto
        {
            Result = new SignResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            },
            Message = "User profile updated successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }


    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public async Task<ResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var principal = await _tokenService.GetPrincipalFromToken(refreshTokenDto.RefreshToken);
        if (principal == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid refresh token",
                StatusCode = 401
            };
        }

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var storedRefreshToken = await _tokenService.RetrieveRefreshToken(userId);

        if (storedRefreshToken == null || storedRefreshToken.Trim() != refreshTokenDto.RefreshToken.Trim())
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid refresh token",
                StatusCode = 401
            };
        }

        // Kiểm tra thời gian hết hạn của refresh token
        var refreshTokenEntity = await _unitOfWork.RefreshTokens.GetTokenByUserIdAsync(userId);

        if (refreshTokenEntity == null || refreshTokenEntity.Expires < DateTime.Now)
        {
            // Nếu refresh token đã hết hạn, yêu cầu người dùng đăng nhập lại
            if (refreshTokenEntity != null)
            {
                await _unitOfWork.RefreshTokens.RemoveTokenAsync(refreshTokenEntity);
                await _unitOfWork.SaveAsync();
            }

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Refresh token expired, please login again",
                StatusCode = 401
            };
        }

        // Nếu token chưa hết hạn, tạo access token mới và giữ lại refresh token cũ
        var userToUpdate = await _userManager.FindByIdAsync(userId);
        var newAccessToken = await _tokenService.GenerateJwtAccessTokenAsync(userToUpdate);

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Token refreshed successfully",
            StatusCode = 200,
            Result = new
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshTokenDto.RefreshToken // Giữ lại refresh token cũ
            }
        };
    }


    /// <summary>
    /// Get infor user
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<ResponseDto> FetchUserByToken(string token)
    {
        try
        {
            // Sử dụng GetPrincipalFromToken để lấy ClaimsPrincipal từ token
            var principal = await _tokenService.GetPrincipalFromToken(token);
            if (principal == null)
            {
                return new ResponseDto()
                {
                    Message = "Invalid token",
                    StatusCode = 401, // Unauthorized
                    IsSuccess = false,
                    Result = null
                };
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ResponseDto()
                {
                    Message = "Invalid user",
                    StatusCode = 400,
                    IsSuccess = false,
                    Result = null
                };
            }

            // Lấy role từ UserManager
            var roles = await _userManager.GetRolesAsync(user);

            // Tạo GetUserDto từ claims
            var userDto = new GetUserDto
            {
                Id = user.Id,
                FullName = principal.FindFirst("FullName")?.Value,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = principal.FindFirst("Address")?.Value,
                Country = principal.FindFirst("Country")?.Value,
                Cccd = principal.FindFirst("Cccd")?.Value,
                BirthDate = DateTime.Parse(principal.FindFirst("BirthDate")?.Value),
                AvatarUrl = principal.FindFirst("AvatarUrl")?.Value,
                UserName = user.UserName,
                Roles = roles.ToList()
            };

            return new ResponseDto()
            {
                Message = "Get info successfully",
                StatusCode = 200,
                IsSuccess = true,
                Result = userDto
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto()
            {
                Message = ex.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }


    /// <summary>
    /// Upload user avatar.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="User"></param>
    /// <returns></returns>
    public async Task<ResponseDto> UploadUserAvatar(IFormFile file, ClaimsPrincipal User)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                throw new Exception("Not authentication!");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new Exception("User does not exist");
            }

            var responseDto = await _firebaseService.UploadImageUser(file, StaticFirebaseFolders.UserAvatars);

            if (!responseDto.IsSuccess)
            {
                throw new Exception("Image upload fail!");
            }

            user.AvatarUrl = responseDto.Result?.ToString();

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new Exception("Update user avatar fail!");
            }

            // Xóa refresh token cũ nếu có
            var existingRefreshToken = await _tokenService.RetrieveRefreshToken(user.Id);
            if (existingRefreshToken != null)
            {
                await _tokenService.DeleteRefreshToken(user.Id);
            }

            //Tạo access token và refresh token mới
            var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
            await _tokenService.StoreRefreshToken(user.Id, refreshToken);

            return new ResponseDto()
            {
                Message = "Upload user avatar successfully!",
                Result = new AvatarTokenDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AvatarUrl = responseDto.Result?.ToString()
                },
                IsSuccess = true,
                StatusCode = 200
            };
        }
        catch (Exception e)
        {
            return new ResponseDto()
            {
                Message = e.Message,
                Result = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }

    /// <summary>
    /// Get user avatar.
    /// </summary>
    /// <param name="User"></param>
    /// <returns></returns>
    public async Task<MemoryStream> GetUserAvatar(ClaimsPrincipal User)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var user = await _userManager.FindByIdAsync(userId!);

        var stream = await _firebaseService.GetImage(user!.AvatarUrl);

        return stream;
    }

    /// <summary>
    /// Send verify email.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="confirmationLink"></param>
    /// <returns></returns>
    public async Task<ResponseDto> SendVerifyEmail(string email, string userId, string token)
    {
        // Gọi EmailService để gửi email xác nhận
        await _emailService.SendVerifyEmail(email, userId, token);
        return new()
        {
            Message = "Send verify email successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = null
        };
    }


    /// <summary>
    /// Verify email.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<ResponseDto> VerifyEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user!.EmailConfirmed)
        {
            return new ResponseDto()
            {
                Message = "Your email has been confirmed!",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };
        }

        string decodedToken = Uri.UnescapeDataString(token);

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
    /// Change Password
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
    /// Forgot Password
    /// </summary>
    private string _ip;

    private string _city;
    private string _region;
    private string _country;
    private const int MaxAttemptsPerDay = 3;

    public async Task<ResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            // Tìm người dùng theo Email/Số điện thoại
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                user = await _userManager.Users.FirstOrDefaultAsync(
                    u => u.PhoneNumber == forgotPasswordDto.Email);
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

                    this._ip = data["ip"].ToString();
                    this._city = data["city"].ToString();
                    this._region = data["region"].ToString();
                    this._country = data["country"].ToString();
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
                currentDate, resetLink, operatingSystem, browser, _ip, _region, _city, _country);

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


    /// <summary>
    /// Reset Password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="token"></param>
    /// <param name="password"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Lock User
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ResponseDto> LockUser(string id)
    {
        var userId = await _userManager.FindByIdAsync(id);
        if (userId == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found.",
                StatusCode = 404
            };
        }

        var userRole = await _userManager.GetRolesAsync(userId);

        if (userRole.Contains(StaticUserRoles.Admin))
        {
            return new ResponseDto()
            {
                Message = "You are not an Admin",
                IsSuccess = false,
                StatusCode = 400,
                Result = null
            };
        }

        userId.LockoutEnd = DateTimeOffset.MaxValue;
        var result = await _userManager.UpdateAsync(userId);
        if (!result.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Lock user was failed",
                IsSuccess = false,
                StatusCode = 400,
                Result = null
            };
        }

        return new ResponseDto()
        {
            Message = "Lock user successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = null
        };
    }

    /// <summary>
    /// Unlock User
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ResponseDto> UnlockUser(string id)
    {
        var userId = await _userManager.FindByIdAsync(id);
        if (userId == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found.",
                StatusCode = 404
            };
        }

        var userRole = await _userManager.GetRolesAsync(userId);

        if (userRole.Contains(StaticUserRoles.Admin))
        {
            return new ResponseDto()
            {
                Message = "You are not an Admin",
                IsSuccess = false,
                StatusCode = 400,
                Result = null
            };
        }

        userId.LockoutEnd = null;
        var result = await _userManager.UpdateAsync(userId);
        if (!result.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Lock user was failed",
                IsSuccess = false,
                StatusCode = 400,
                Result = null
            };
        }

        return new ResponseDto()
        {
            Message = "Lock user successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = null
        };
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterOn"></param>
    /// <param name="filterQuery"></param>
    /// <param name="sortBy"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<ResponseDto> GetAllUsers(
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        #region MyRegion

        try
        {
            var usersQuery = _userManager.Users.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "fullname":
                        usersQuery = usersQuery.Where(u => u.FullName.Contains(filterQuery));
                        break;
                    case "email":
                        usersQuery = usersQuery.Where(u => u.Email.Contains(filterQuery));
                        break;
                    case "phonenumber":
                        usersQuery = usersQuery.Where(u => u.PhoneNumber.Contains(filterQuery));
                        break;
                    case "address":
                        usersQuery = usersQuery.Where(u => u.Address.Contains(filterQuery));
                        break;
                    case "country":
                        usersQuery = usersQuery.Where(u => u.Country.Contains(filterQuery));
                        break;
                    case "cccd":
                        usersQuery = usersQuery.Where(u => u.Cccd.Contains(filterQuery));
                        break;
                    case "birthdate":
                        if (DateTime.TryParse(filterQuery, out DateTime birthDate))
                        {
                            usersQuery = usersQuery.Where(u => u.BirthDate.Date == birthDate.Date);
                        }

                        break;
                    case "username":
                        usersQuery = usersQuery.Where(u => u.UserName.Contains(filterQuery));
                        break;
                    default:
                        usersQuery = usersQuery.Where(u => u.FullName.Contains(filterQuery));
                        break;
                }
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "fullname":
                        usersQuery = usersQuery.OrderBy(u => u.FullName);
                        break;
                    case "email":
                        usersQuery = usersQuery.OrderBy(u => u.Email);
                        break;
                    case "phonenumber":
                        usersQuery = usersQuery.OrderBy(u => u.PhoneNumber);
                        break;
                    case "address":
                        usersQuery = usersQuery.OrderBy(u => u.Address);
                        break;
                    case "country":
                        usersQuery = usersQuery.OrderBy(u => u.Country);
                        break;
                    case "cccd":
                        usersQuery = usersQuery.OrderBy(u => u.Cccd);
                        break;
                    case "birthdate":
                        usersQuery = usersQuery.OrderBy(u => u.BirthDate);
                        break;
                    case "username":
                        usersQuery = usersQuery.OrderBy(u => u.UserName);
                        break;
                    default:
                        usersQuery = usersQuery.OrderBy(u => u.FullName);
                        break;
                }
            }

            // Pagination
            var totalUsers = await usersQuery.CountAsync(); // Đếm tổng số người dùng
            var users = await usersQuery
                .Skip((pageNumber - 1) * pageSize) // Tính số bản ghi cần bỏ qua
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToListAsync(); // Thực thi truy vấn

            #endregion Query Parameters

            if (users == null || !users.Any())
            {
                return new ResponseDto()
                {
                    Message = "There are no users",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var userInfoDtoList = new List<GetUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userInfoDto = new GetUserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Country = user.Country,
                    Cccd = user.Cccd,
                    BirthDate = user.BirthDate,
                    AvatarUrl = user.AvatarUrl,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                };

                userInfoDtoList.Add(userInfoDto);
            }

            return new ResponseDto()
            {
                Message = "Get all users successfully",
                Result = userInfoDtoList,
                IsSuccess = true,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto()
            {
                Message = ex.Message,
                Result = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }
}