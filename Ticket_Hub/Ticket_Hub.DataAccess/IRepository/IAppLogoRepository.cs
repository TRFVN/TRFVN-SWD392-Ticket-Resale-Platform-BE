using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IAppLogoRepository : IRepository<AppLogo>
{
    void Update(AppLogo appLogo);
    void UpdateRange(IEnumerable<AppLogo> appLogos);
}