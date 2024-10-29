using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IPrivacyRepository : IRepository<Privacy>
{
    void Update(Privacy privacy);
    void UpdateRange(IEnumerable<Privacy> privacies);
}