using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Negotiation;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class NegotiationsService : INegotiationsService
{
    private readonly IUnitOfWork _unitOfWork;

    public NegotiationsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<ResponseDto> GetNegotiationsServices
    (ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetNegotiations(ClaimsPrincipal user, Guid negotiationsId)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> CreateNegotiations(ClaimsPrincipal user, CreateNegotiationsDto createNegotiationsDto)
    {
        var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return new ResponseDto
            {
                Message = "User not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var negotiations = new Negotiations
        {
            ChatRoomId = createNegotiationsDto.ChatRoomId,
            MessageId = createNegotiationsDto.MessageId,
            TicketId = createNegotiationsDto.TicketId,
            Price = createNegotiationsDto.Price,
            Status = false
        };

        await _unitOfWork.NegotiationsRepository.AddAsync(negotiations);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Negotiation created successfully",
            Result = null,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> AcceptNegotiations(ClaimsPrincipal user, Guid negotiationsId)
    {
        var nego = await _unitOfWork.NegotiationsRepository.GetAsync(n => n.NegotiationId == negotiationsId);
        if (nego == null)
        {
            return new ResponseDto
            {
                Message = "Negotiation not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        // Cập nhật trạng thái của Negotiation
        nego.Status = true;

        // Lấy Ticket liên quan và cập nhật thông tin
        var ticket = await _unitOfWork.TicketRepository.GetAsync(t => t.TicketId == nego.TicketId);
        if (ticket == null)
        {
            return new ResponseDto
            {
                Message = "Ticket not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        // Cập nhật NewPrice và NegotiationStatus của Ticket
        ticket.NewPrice = nego.Price;
        ticket.NegotiationStatus = true;

        // Lưu các thay đổi vào cơ sở dữ liệu
        _unitOfWork.TicketRepository.Update(ticket);
        _unitOfWork.NegotiationsRepository.Update(nego);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Negotiation accepted and ticket price updated",
            Result = null,
            IsSuccess = true,
            StatusCode = 200
        };
    }
}