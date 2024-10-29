using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class TermOfUseRepository : Repository<TermOfUse>, ITermOfUseRepository
{
    private readonly ApplicationDbContext _context;

    public TermOfUseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(TermOfUse termOfUse)
    {
        _context.TermOfUses.Update(termOfUse);
    }

    public void UpdateRange(IEnumerable<TermOfUse> termOfUses)
    {
        _context.TermOfUses.UpdateRange(termOfUses);
    }
}