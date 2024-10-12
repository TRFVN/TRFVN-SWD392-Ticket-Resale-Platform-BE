using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class LocationRepository : Repository<Location>, ILocationRepository
{
    private readonly ApplicationDbContext _context;
    
    public LocationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Location location)
    {
        _context.Locations.Update(location);
    }

    public void UpdateRange(IEnumerable<Location> locations)
    {
        _context.Locations.UpdateRange(locations);
    }

    public async Task<Location> GetById(Guid locationId)
    {
        return await _context.Locations.FirstOrDefaultAsync(x => x.LocationId == locationId);
    }
}