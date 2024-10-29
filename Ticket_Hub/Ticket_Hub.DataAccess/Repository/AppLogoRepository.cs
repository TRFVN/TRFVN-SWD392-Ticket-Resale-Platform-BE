using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class AppLogoRepository : Repository<AppLogo>, IAppLogoRepository
{ 
    private readonly ApplicationDbContext _context;

    public AppLogoRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(AppLogo appLogo)
    {
        _context.AppLogos.Update(appLogo);
    }

    public void UpdateRange(IEnumerable<AppLogo> appLogos)
    {
        _context.AppLogos.UpdateRange(appLogos);
    }
}