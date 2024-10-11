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
        public async Task<Favourite?> GetByIdAsync(Guid id)
        {
            return await _context.Favourites.FirstOrDefaultAsync(f => f.FavouriteId == id);
        }

        public void UpdateFavourite(Favourite favourite)
        {
            _context.Favourites.Update(favourite);
        }
    }
}
