using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ILocationRepository : IRepository<Location>
{
    void Update(Location location);
    void UpdateRange(IEnumerable<Location> locations);
    Task<Location> GetById(Guid locationId);
}