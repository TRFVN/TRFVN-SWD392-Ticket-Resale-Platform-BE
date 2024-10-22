using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository
{
    public interface IFavouriteRepository : IRepository<Favourite>
    {
        void Update(Favourite favouriteId);
        void UpdateRange(IEnumerable<Favourite> favourites);
        Task<Favourite> GetById(Guid favouriteId);
    }
}
