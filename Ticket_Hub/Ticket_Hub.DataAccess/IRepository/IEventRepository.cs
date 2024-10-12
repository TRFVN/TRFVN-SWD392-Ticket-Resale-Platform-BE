using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IEventRepository : IRepository<Event>
{
    void Update(Event events);
    void UpdateRange(IEnumerable<Event> events);
    Task<Event> GetById(Guid eventId);
}