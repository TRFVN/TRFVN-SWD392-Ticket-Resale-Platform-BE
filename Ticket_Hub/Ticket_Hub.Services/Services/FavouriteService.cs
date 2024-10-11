using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavouriteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateFavouriteAsync(Favourite favourite)
        {
            await _unitOfWork.Favourite.AddAsync(favourite);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteFavouriteAsync(Guid id)
        {
            var favourite = await _unitOfWork.Favourite.GetByIdAsync(id);
            if (favourite != null)
            {
                _unitOfWork.Favourite.Remove(favourite);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<IEnumerable<Favourite>> GetAllFavouritesAsync()
        {
            return await _unitOfWork.Favourite.GetAllAsync();
        }

        public async Task<Favourite?> GetFavouriteByIdAsync(Guid id)
        {
            return await _unitOfWork.Favourite.GetByIdAsync(id);
        }

        public async Task UpdateFavouriteAsync(Favourite favourite)
        {
            _unitOfWork.Favourite.Update(favourite);
            await _unitOfWork.SaveAsync();
        }
    }
}
