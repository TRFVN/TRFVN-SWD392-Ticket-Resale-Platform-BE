using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository
{
    public class CartHeaderRepository : Repository<CartHeader>, ICartHeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public CartHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CartHeader cartHeader)
        {
            _context.CartHeaders.Update(cartHeader);
        }

        public void UpdateRange(IEnumerable<CartHeader> cartHeaders)
        {
            _context.CartHeaders.UpdateRange(cartHeaders);
        }

        public async Task<CartHeader> GetById(Guid cartHeaderId)
        {
            return await _context.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartHeaderId);
        }
    }
}
