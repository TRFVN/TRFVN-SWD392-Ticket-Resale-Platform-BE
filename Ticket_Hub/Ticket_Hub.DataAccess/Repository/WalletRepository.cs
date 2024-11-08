using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class WalletRepository : Repository<Wallet>, IWalletRepository
{
    private readonly ApplicationDbContext _context;

    public WalletRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
    }

    public void UpdateRange(IEnumerable<Wallet> wallets)
    {
        _context.Wallets.UpdateRange(wallets);
    }

    public async Task<Wallet> GetById(Guid walletId)
    {
        return await _context.Wallets.FirstOrDefaultAsync(x => x.WalletId == walletId);
    }
}