using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company company);
    void UpdateRange(IEnumerable<Company> companies);
}