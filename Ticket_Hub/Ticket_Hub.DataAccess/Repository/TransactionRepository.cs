using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class TransactionRepository : Repository<Transactions>, ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Transactions transaction)
    {
        _context.Transactions.Update(transaction);
    }

    public void UpdateRange(IEnumerable<Transactions> transactions)
    {
        _context.Transactions.UpdateRange(transactions);
    }

    public async Task<Transactions> GetById(Guid transactionId)
    {
        return await _context.Transactions.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
    }
}