using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface ICartItemRepository : IRepository<CartItem>
{
    void Update(CartItem cartItem);
    void UpdateRange(IEnumerable<CartItem> cartItems);
}