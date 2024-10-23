using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.CartDetail;
using Ticket_Hub.Models.DTO.CartHeader;

namespace Ticket_Hub.Services.IServices
{
    public interface ICartDetailService
    {
        Task<ResponseDto> GetCartDetails
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

        Task<ResponseDto> GetCartDetail(ClaimsPrincipal user, Guid cartDetailId);
        Task<ResponseDto> CreateCartDetail(ClaimsPrincipal user, CreateCartDetailDto createCartDetailDto);
        Task<ResponseDto> UpdateCartDetail(ClaimsPrincipal user, UpdateCartDetailDto updateCartDetailDto);
        Task<ResponseDto> DeleteCartDetail(ClaimsPrincipal user, Guid cartDetailId);
    }
}
