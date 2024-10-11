using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Services.IServices
{
    public interface IFavouriteService
    {
        Task<IEnumerable<Favourite>> GetAllFavouritesAsync();
        Task<Favourite?> GetFavouriteByIdAsync(Guid id);
        Task CreateFavouriteAsync(Favourite favourite);
        Task UpdateFavouriteAsync(Favourite favourite);
        Task DeleteFavouriteAsync(Guid id);
    }
}
