using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Event;
using Ticket_Hub.Models.DTO.Negotiation;

namespace Ticket_Hub.Services.IServices;

public interface INegotiationsService
{
    Task<ResponseDto> GetNegotiationsServices
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

    Task<ResponseDto> GetNegotiations(ClaimsPrincipal user, Guid negotiationsId);
    Task<ResponseDto> CreateNegotiations(ClaimsPrincipal user, CreateNegotiationsDto createNegotiationsDto);
    Task<ResponseDto> AcceptNegotiations(ClaimsPrincipal user, Guid negotiationsId);
}