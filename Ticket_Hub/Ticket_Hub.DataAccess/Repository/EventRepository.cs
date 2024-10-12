using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class EventRepository : Repository<Event>, IEventRepository
{
    private readonly ApplicationDbContext _context;
    
    public EventRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public void Update(Event location)
    {
        _context.Events.Update(location);
    }

    public void UpdateRange(IEnumerable<Event> events)
    {
        _context.Events.UpdateRange(events);
    }

    public async Task<Event> GetById(Guid eventId)
    {
        return await _context.Events.FirstOrDefaultAsync(x => x.EventId == eventId);
    }
}