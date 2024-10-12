using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Location;
using Ticket_Hub.Models.DTO.Ticket;

namespace Ticket_Hub.Services.IServices;

public interface ILocationService
{
    Task<ResponseDto> GetLocations
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

    Task<ResponseDto> GetLocation(ClaimsPrincipal user, Guid locationId);
    Task<ResponseDto> CreateLocation(ClaimsPrincipal user, CreateLocationDto createLocationDto);
    Task<ResponseDto> UpdateLocation(ClaimsPrincipal user, UpdateLocationDto updateLocationDto);
    Task<ResponseDto> DeleteLocation(ClaimsPrincipal user, Guid locationId);
}