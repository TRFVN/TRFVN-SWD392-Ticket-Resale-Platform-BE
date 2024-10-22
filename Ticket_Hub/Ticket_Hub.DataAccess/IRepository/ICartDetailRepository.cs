using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository
{
    public interface ICartDetailRepository : IRepository<CartDetail>
    {
        void Update(CartDetail cartDetail);
        void UpdateRange(IEnumerable<CartDetail> cartDetails);
        Task<CartDetail> GetById(Guid cartDetailId);
        public List<CartDetail> GetAll(Guid cartHeaderId);
    }
}
