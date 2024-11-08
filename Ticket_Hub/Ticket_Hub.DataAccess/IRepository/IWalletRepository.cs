using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IWalletRepository : IRepository<Wallet>
{
    void Update(Wallet wallet);
    void UpdateRange(IEnumerable<Wallet> wallets);
    Task<Wallet> GetById(Guid walletId);
}