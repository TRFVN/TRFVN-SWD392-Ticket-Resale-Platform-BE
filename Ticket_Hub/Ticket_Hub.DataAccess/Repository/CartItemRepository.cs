using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class CartItemRepository : Repository<CartItem>, ICartItemRepository
{
    private readonly ApplicationDbContext _context;
    public CartItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
    }

    public void UpdateRange(IEnumerable<CartItem> cartItems)
    {
        _context.CartItems.UpdateRange(cartItems);
    }
    
    public async Task<CartItem> GetCartItemWithTicketAsync(Guid cartId, Guid ticketId)
    {
        return await _context.CartItems
            .Include(ci => ci.Ticket)
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.TicketId == ticketId);
    }
}