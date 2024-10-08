using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IRefreshTokenRepository : IRepository<RefreshTokens>
{
    Task<RefreshTokens> GetTokenByUserIdAsync(string userId);
    Task AddTokenAsync(RefreshTokens token);
    Task RemoveTokenAsync(RefreshTokens token);
}