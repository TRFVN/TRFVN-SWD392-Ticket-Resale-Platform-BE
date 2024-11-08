using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ITransactionRepository : IRepository<Transactions>
{
    void Update(Transactions transaction);
    void UpdateRange(IEnumerable<Transactions> transactions);
    Task<Transactions> GetById(Guid transactionId);
}