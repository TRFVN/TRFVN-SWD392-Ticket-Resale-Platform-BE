using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class RefreshTokenRepository : Repository<RefreshTokens>, IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<RefreshTokens> GetTokenByUserIdAsync(string userId)
    {
        return await _context.RefreshTokens
            .Where(t => t.UserId == userId && t.Expires > DateTime.Now)
            .FirstOrDefaultAsync();
    }

    public async Task AddTokenAsync(RefreshTokens token)
    {
        await _context.RefreshTokens.AddAsync(token);
    }

    public async Task RemoveTokenAsync(RefreshTokens token)
    {
        _context.RefreshTokens.Remove(token);
    }
}