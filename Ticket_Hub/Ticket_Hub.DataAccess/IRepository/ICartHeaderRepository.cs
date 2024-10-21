using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository
{
    public interface ICartHeaderRepository : IRepository<CartHeader>
    {
        void Update(CartHeader cartHeader);
        void UpdateRange(IEnumerable<CartHeader> cartHeaders);
        Task<CartHeader> GetById(Guid cartHeaderId);
    }
}
