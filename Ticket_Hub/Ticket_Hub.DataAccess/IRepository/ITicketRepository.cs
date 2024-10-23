using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        void Update(Ticket ticket);
        void UpdateRange(IEnumerable<Ticket> tickets);
        Task<Ticket> GeTicketById(Guid ticketId);
        Task<IEnumerable<Ticket>> GetAllWithEventAndLocationAsync();
        Task<int> SaveAsync();
    }
}