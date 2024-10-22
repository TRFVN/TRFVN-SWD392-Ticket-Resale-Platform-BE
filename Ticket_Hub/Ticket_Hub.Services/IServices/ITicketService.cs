using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Ticket;

namespace Ticket_Hub.Services.IServices;

public interface ITicketService
{
    Task<ResponseDto> GetTickets
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

    Task<ResponseDto> GetTicket(ClaimsPrincipal user, Guid ticketId);
    Task<ResponseDto> CreateTicket(ClaimsPrincipal user, CreateTicketDto createTicketDto);
    Task<ResponseDto> UpdateTicket(ClaimsPrincipal user, UpdateTicketDto updateTicketDto);
    Task<ResponseDto> DeleteTicket(ClaimsPrincipal user, Guid ticketId);
    Task<ResponseDto> UploadTicketImage(ClaimsPrincipal user, Guid ticketId, UploadTicketImgDto uploadTicketImgDto);
    Task<MemoryStream> GetTicketImage(ClaimsPrincipal user, Guid ticketId);
    Task<ResponseDto> AcceptTicket(ClaimsPrincipal user, Guid ticketId);
    Task<ResponseDto> RejectTicket(ClaimsPrincipal user, Guid ticketId);
}