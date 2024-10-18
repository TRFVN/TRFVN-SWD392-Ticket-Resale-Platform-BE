using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO.Favourite;
using Ticket_Hub.Models.DTO;

namespace Ticket_Hub.Services.IServices
{
    public interface IFavouriteService
    {
        Task<ResponseDto> GetFavourites
            (
                ClaimsPrincipal user,
                string? filterOn,
                string? filterQuery,
                string? sortBy,
                int pageNumber = 0,
                int pageSize = 0
            );
        Task<ResponseDto> GetFavourite(ClaimsPrincipal user, Guid favouriteId);
        Task<ResponseDto> CreateFavourite(ClaimsPrincipal user, CreateFavouriteDto createFavouriteDto);
        Task<ResponseDto> UpdateFavourite(ClaimsPrincipal user, UpdateFavouriteDto updateFavouriteDto);
        Task<ResponseDto> DeleteFavourite(ClaimsPrincipal user, Guid favouriteId);
    }
}
