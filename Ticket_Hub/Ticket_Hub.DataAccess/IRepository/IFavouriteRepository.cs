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
        Task<Favourite?> GetByIdAsync(Guid id);
        void UpdateFavourite(Favourite favourite);
    }
}
