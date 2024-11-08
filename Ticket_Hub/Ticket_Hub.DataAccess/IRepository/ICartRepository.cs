using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ICartRepository : IRepository<Cart>
{
    void Update(Cart cart);
    void UpdateRange(IEnumerable<Cart> carts);
}