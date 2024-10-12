using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        
        public TicketRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Ticket> GeTicketById(Guid ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(x => x.TicketId == ticketId);
        }

        public void Update(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
        }

        public void UpdateRange(IEnumerable<Ticket> tickets)
        {
            _context.Tickets.UpdateRange(tickets);
        }
    }
}
