using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Event;

namespace Ticket_Hub.Services.IServices;

public interface IEventService
{
    Task<ResponseDto> GetEvents
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

    Task<ResponseDto> GetEvent(ClaimsPrincipal user, Guid eventId);
    Task<ResponseDto> CreateEvent(ClaimsPrincipal user, CreateEventDto createEventDto);
    Task<ResponseDto> UpdateEvent(ClaimsPrincipal user, UpdateEventDto updateEventDto);
    Task<ResponseDto> DeleteEvent(ClaimsPrincipal user, Guid eventId);
}