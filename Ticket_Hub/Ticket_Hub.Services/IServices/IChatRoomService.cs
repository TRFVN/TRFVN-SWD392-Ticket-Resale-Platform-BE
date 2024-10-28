using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.ChatRoom;

namespace Ticket_Hub.Services.IServices;

public interface IChatRoomService
{
    Task<ResponseDto> GetChatRooms
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

    Task<ResponseDto> GetChatRoom(ClaimsPrincipal user, Guid userId);
    Task<ResponseDto> CreateChatRoom(ClaimsPrincipal user, CreateChatRoomDto createChatRoomDto);
    Task<ResponseDto> UpdateChatRoom(ClaimsPrincipal user, UpdateChatRoomDto updateChatRoomDto);
    Task<ResponseDto> DeleteChatRoom(ClaimsPrincipal user, Guid chatRoomId);
}