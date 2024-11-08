using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class CartRepository : Repository<Cart>, ICartRepository
{
    private readonly ApplicationDbContext _context;
    public CartRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Cart carts)
    {
        _context.Carts.Update(carts);
    }

    public void UpdateRange(IEnumerable<Cart> carts)
    {
        _context.Carts.UpdateRange(carts);
    }
}