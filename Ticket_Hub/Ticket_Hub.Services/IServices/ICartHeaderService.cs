using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.CartHeader;
using Ticket_Hub.Models.DTO.Category;

namespace Ticket_Hub.Services.IServices
{
    public interface ICartHeaderService
    {
        Task<ResponseDto> GetCartHeaders
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );
        Task<ResponseDto> GetCartHeader(ClaimsPrincipal user, Guid cartHeaderId);
        Task<ResponseDto> CreateCartHeader(ClaimsPrincipal user, CreateCartHeaderDto createCartHeaderDto);
        Task<ResponseDto> UpdateCartHeader(ClaimsPrincipal user, UpdateCartHeaderDto updateCartHeaderDto);
        Task<ResponseDto> DeleteCartHeader(ClaimsPrincipal user, Guid cartHeaderId);
    }
}
