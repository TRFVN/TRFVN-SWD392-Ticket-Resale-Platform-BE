using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class OrderRepository :  Repository<Orders>, IOrderRepository
{
    private readonly ApplicationDbContext _context;
    
    
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Orders orders)
    {
        throw new NotImplementedException();
    }

    public void UpdateRange(IEnumerable<Orders> orders)
    {
        throw new NotImplementedException();
    }

    public Task<Orders> GetById(Guid orderId)
    {
        throw new NotImplementedException();
    }
}