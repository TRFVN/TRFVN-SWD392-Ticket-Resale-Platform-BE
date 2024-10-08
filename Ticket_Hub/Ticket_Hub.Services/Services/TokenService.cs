using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }


    //Create AccessToken
    public async Task<string> GenerateJwtAccessTokenAsync(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));
        var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

        var tokenObject = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(60),//Expiration time is 1h
            claims: authClaims,//list of rights
            signingCredentials: signingCredentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);

        return accessToken;
    }


    //Create RefreshToken
    public async Task<string> GenerateJwtRefreshTokenAsync(ApplicationUser user)
    {
        // Create a list of claims containing user information
        var authClaims = new List<Claim>()
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
    };

        // Create cryptographic objects for tokens
        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));
        var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

        // Create JWT token object
        var tokenObject = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(3), //Expiration time is 3 days
            claims: authClaims,
            signingCredentials: signingCredentials
        );

        // Token generation successful
        var refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);

        // Create expiration time
        var expires = DateTime.Now.AddDays(3);

        // Create object to save in database
        var tokenEntity = new RefreshTokens
        {
            RefreshTokensId = Guid.NewGuid(),
            UserId = user.Id,
            RefreshToken = refreshToken,
            Expires = expires,
            CreatedBy = user.UserName,
            CreatedTime = DateTime.Now,
            UpdatedBy = user.UserName,
            UpdatedTime = DateTime.Now,
            Status = 1
        };

        // Lưu token vào database
        await _unitOfWork.RefreshTokens.AddTokenAsync(tokenEntity);
        await _unitOfWork.SaveAsync();

        return refreshToken;
    }

    public async Task<ClaimsPrincipal> GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }


    //Store RefreshToken
    public async Task<bool> StoreRefreshToken(string userId, string refreshToken)
    {
        var existingToken = await _unitOfWork.RefreshTokens.GetTokenByUserIdAsync(userId);
        await _unitOfWork.RefreshTokens.RemoveTokenAsync(existingToken);

        var tokenEntity = new RefreshTokens
        {
            UserId = userId,
            RefreshToken = refreshToken,
            Expires = DateTime.Now.AddDays(3), 
            CreatedBy = userId,
            CreatedTime = DateTime.Now,
            UpdatedBy = userId,
            UpdatedTime = DateTime.Now,
            Status = 1 
        };

        await _unitOfWork.RefreshTokens.AddTokenAsync(tokenEntity);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<string> RetrieveRefreshToken(string userId)
    {
        var tokenEntity = await _unitOfWork.RefreshTokens.GetTokenByUserIdAsync(userId);
        return tokenEntity?.RefreshToken;
    }

    public async Task<bool> DeleteRefreshToken(string userId)
    {
        var existingToken = await _unitOfWork.RefreshTokens.GetTokenByUserIdAsync(userId);
        await _unitOfWork.RefreshTokens.RemoveTokenAsync(existingToken);
        await _unitOfWork.SaveAsync();
        return true;

        return false;
    }
}