using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ITicketDetailRepository : IRepository<TicketDetail>
{
    void Update(TicketDetail ticketDetail);
    void UpdateRange(IEnumerable<TicketDetail> ticketDetails);
    Task<TicketDetail> GetTicketDetailById(Guid ticketDetailId);
}