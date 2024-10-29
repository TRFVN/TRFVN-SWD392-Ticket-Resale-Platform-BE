using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ITermOfUseRepository : IRepository<TermOfUse>
{
    void Update(TermOfUse termOfUse);
    void UpdateRange(IEnumerable<TermOfUse> termOfUses);
}