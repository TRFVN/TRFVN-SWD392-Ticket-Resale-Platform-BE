using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class PrivacyRepository : Repository<Privacy>, IPrivacyRepository
{
    private readonly ApplicationDbContext _context;

    public PrivacyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Privacy privacy)
    {
        _context.Privacies.Update(privacy);
    }

    public void UpdateRange(IEnumerable<Privacy> privacies)
    {
        _context.Privacies.UpdateRange(privacies);
    }
}