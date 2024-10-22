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
    public class FavouriteRepository : Repository<Favourite>, IFavouriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavouriteRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Favourite favourite)
        {
            _context.Favourites.Update(favourite);
        }

        public void UpdateRange(IEnumerable<Favourite> favourites)
        {
            _context.Favourites.UpdateRange(favourites);
        }

        public async Task<Favourite> GetById(Guid favouriteId)
        {
            return await _context.Favourites.FirstOrDefaultAsync(x => x.FavouriteId == favouriteId);
        }
    }
}
