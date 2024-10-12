using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Ticket;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class TicketService : ITicketService
{
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private UserManager<ApplicationUser> _userManager;

    public TicketService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ResponseDto> GetTickets
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    )
    {
        // Lấy tất cả các vé có trong database
        var tickets = await _unitOfWork.TicketRepository.GetAllAsync();

        // Kiểm tra nếu danh sách tickets là null hoặc rỗng
        if (!tickets.Any())
        {
            return new ResponseDto()
            {
                Message = "There are no tickets",
                IsSuccess = true,
                StatusCode = 404,
                Result = null
            };
        }

        var listTickets = tickets.ToList();

        // Filter Query
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            switch (filterOn.Trim().ToLower())
            {
                case "ticketname":
                    listTickets = listTickets.Where(x =>
                        x.TicketName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;

                case "ticketprice":
                    if (double.TryParse(filterQuery, out double price))
                    {
                        listTickets = listTickets.Where(x => x.TicketPrice == price).ToList();
                    }

                    break;

                case "ticketquantity":
                    if (int.TryParse(filterQuery, out int quantity))
                    {
                        listTickets = listTickets.Where(x => x.TicketQuantity == quantity).ToList();
                    }

                    break;

                case "ticketdescription":
                    listTickets = listTickets.Where(x =>
                        x.TicketDescription.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;

                default:
                    break;
            }
        }

        // Sort Query
        if (!string.IsNullOrEmpty(sortBy))
        {
            switch (sortBy.Trim().ToLower())
            {
                case "ticketname":
                    listTickets = listTickets.OrderBy(x => x.TicketName).ToList();
                    break;

                case "ticketprice":
                    listTickets = listTickets.OrderBy(x => x.TicketPrice).ToList();
                    break;

                default:
                    // Nếu không có giá trị `sortBy` hợp lệ, sắp xếp theo `TicketPrice` giảm dần
                    listTickets = listTickets.OrderByDescending(x => x.TicketPrice).ToList();
                    break;
            }
        }
        else
        {
            // Trường hợp không có `sortBy` nào được chỉ định
            listTickets = listTickets.OrderByDescending(x => x.TicketPrice).ToList();
        }

        // Phân trang
        if (pageNumber > 0 && pageSize > 0)
        {
            var skipResult = (pageNumber - 1) * pageSize;
            listTickets = listTickets.Skip(skipResult).Take(pageSize).ToList();
        }

        // Chuyển đổi danh sách vé thành DTO
        var ticketsDto = listTickets.Select(ticket => new GetTicketDto()
        {
            TicketId = ticket.TicketId,
            TicketName = ticket.TicketName,
            EventId = ticket.EventId,
            UserId = ticket.UserId,
            CategoryId = ticket.CategoryId,
            TicketPrice = ticket.TicketPrice,
            TicketQuantity = ticket.TicketQuantity,
            TicketDescription = ticket.TicketDescription,
            SerialNumber = ticket.SerialNumber,
            Status = ticket.Status
        }).ToList();

        return new ResponseDto()
        {
            Message = "Get Tickets successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = ticketsDto
        };
    }

    public async Task<ResponseDto> GetTicket(ClaimsPrincipal user, Guid ticketId)
    {
        var ticketById = await _unitOfWork.TicketRepository.GeTicketById(ticketId);
        if (ticketById == null)
        {
            return new ResponseDto
            {
                Message = "Ticket not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        GetTicketDto ticketDto;
        ticketDto = _mapper.Map<GetTicketDto>(ticketById);

        return new ResponseDto
        {
            Message = "Get Ticket successfully",
            Result = ticketDto,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> CreateTicket(ClaimsPrincipal user, CreateTicketDto createTicketDto)
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

        Ticket ticket = new Ticket()
        {
            TicketId = Guid.NewGuid(),
            TicketName = createTicketDto.TicketName,
            EventId = createTicketDto.EventId,
            UserId = userId,
            CategoryId = createTicketDto.CategoryId,
            TicketPrice = createTicketDto.TicketPrice,
            TicketQuantity = createTicketDto.TicketQuantity,
            TicketDescription = createTicketDto.TicketDescription,
            SerialNumber = createTicketDto.SerialNumber,
            Status = createTicketDto.Status
        };

        await _unitOfWork.TicketRepository.AddAsync(ticket);
        await _unitOfWork.SaveAsync();

        return new ResponseDto()
        {
            Message = "Ticket created successfully",
            IsSuccess = true,
            StatusCode = 201,
            Result = ticket
        };
    }

    public async Task<ResponseDto> UpdateTicket(ClaimsPrincipal user, UpdateTicketDto updateTicketDto)
    {
        var ticketId = await _unitOfWork.TicketRepository.GetAsync(c => c.TicketId == updateTicketDto.TicketId);

        // kiểm tra xem có null không
        if (ticketId == null)
        {
            return new ResponseDto
            {
                Message = "Ticket not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        // cập nhật thông tin danh mục
        ticketId.TicketName = updateTicketDto.TicketName;
        ticketId.TicketDescription = updateTicketDto.TicketDescription;
        ticketId.TicketPrice = updateTicketDto.TicketPrice;
        ticketId.TicketQuantity = updateTicketDto.TicketQuantity;
        ticketId.SerialNumber = updateTicketDto.SerialNumber;
        ticketId.EventId = updateTicketDto.EventId;
        ticketId.CategoryId = updateTicketDto.CategoryId;
        ticketId.Status = 1;


        // thay đổi dữ liệu
        _unitOfWork.TicketRepository.Update(ticketId);

        // lưu thay đổi vào cơ sở dữ liệu
        var save = await _unitOfWork.SaveAsync();
        if (save <= 0)
        {
            return new ResponseDto
            {
                Message = "Failed to update ticket",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        return new ResponseDto
        {
            Message = "Ticket updated successfully",
            Result = ticketId,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> DeleteTicket(ClaimsPrincipal user, Guid ticketId)
    {
        var tId = await _unitOfWork.TicketRepository.GetAsync(c => c.TicketId == ticketId);

        if (tId == null)
        {
            return new ResponseDto
            {
                Message = "Delete level successfully",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        tId.Status = 1;
        //ticketID.UpdatedBy = User.Identity.Name;
        //ticketID.UpdatedTime = DateTime.Now;

        _unitOfWork.TicketRepository.Update(tId);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Ticket deleted successfully",
            Result = tId,
            IsSuccess = true,
            StatusCode = 201
        };
    }
}