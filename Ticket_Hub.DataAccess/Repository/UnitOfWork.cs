using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;

namespace Ticket_Hub.DataAccess.Repository;

public class UnitOfWork: IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}