using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository
{
    public class CartDetailRepository : Repository<CartDetail>, ICartDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public CartDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CartDetail cartDetail)
        {
            _context.CartDetails.Update(cartDetail);
        }

        public void UpdateRange(IEnumerable<CartDetail> cartDetails)
        {
            _context.CartDetails.UpdateRange(cartDetails);
        }

        public async Task<CartDetail> GetById(Guid cartDetailId)
        {
            return await _context.CartDetails.FirstOrDefaultAsync(x => x.CartDetailId == cartDetailId);
        }

        public List<CartDetail> GetAll(Guid cartHeaderId)
        {
            return _context.CartDetails.Include(x => x.CartHeaderId == cartHeaderId).ToList();
        }
    }
}
