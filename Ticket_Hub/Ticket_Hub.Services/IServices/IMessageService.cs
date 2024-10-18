using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO.MemberRating;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Message;

namespace Ticket_Hub.Services.IServices
{
    public interface IMessageService
    {
        Task<ResponseDto> GetMessages
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

        Task<ResponseDto> GetMessage(ClaimsPrincipal user, Guid messageId);

        Task<ResponseDto> CreateMessage(ClaimsPrincipal user, CreateMessageDto createMessageDto);

        Task<ResponseDto> UpdateMessage(ClaimsPrincipal user, UpdateMessageDto updateMessageDto);

        Task<ResponseDto> DeleteMessage(ClaimsPrincipal user, Guid messageId);
    }
}
