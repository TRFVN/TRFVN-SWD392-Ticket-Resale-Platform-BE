using Ticket_Hub.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IOrderRepository : IRepository<Orders>
{
    void Update(Orders orders);
    void UpdateRange(IEnumerable<Orders> orders);
    Task<Orders> GetById(Guid orderId);
}