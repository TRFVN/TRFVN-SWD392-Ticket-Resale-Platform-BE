using System.Security.Claims;
using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.ChatRoom;
using Ticket_Hub.Models.DTO.Event;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class ChatRoomService : IChatRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public ChatRoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GetChatRooms
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        IEnumerable<ChatRoom> allChatRooms = null!;

        allChatRooms = await _unitOfWork.ChatRoomRepository.GetAllAsync();

        if (!allChatRooms.Any())
        {
            return new ResponseDto()
            {
                Message = "There are no chat rooms",
                IsSuccess = true,
                StatusCode = 404,
                Result = null
            };
        }

        var listChatRooms = allChatRooms.ToList();

        // Filter Query
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            switch (filterOn.Trim().ToLower())
            {
                case "roomname":
                    listChatRooms = listChatRooms.Where(x =>
                        x.NameRoom.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                default:
                    break;
            }
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            var sortParams = sortBy.Trim().ToLower().Split('_'); 
            var sortField = sortParams[0]; 
            var sortDirection = sortParams.Length > 1 ? sortParams[1] : "asc"; 

            switch (sortField)
            {
                case "roomname":
                    listChatRooms = sortDirection == "desc"
                        ? listChatRooms.OrderByDescending(x => x.NameRoom).ToList()
                        : listChatRooms.OrderBy(x => x.NameRoom).ToList();
                    break;
                
                default:
                    listChatRooms = listChatRooms.OrderBy(x => x.CreateTime).ToList();
                    break;
            }
        }
        else
        {
            listChatRooms = listChatRooms.OrderBy(x => x.CreateTime).ToList();
        }

        if (pageNumber > 0 && pageSize > 0)
        {
            var skipResult = (pageNumber - 1) * pageSize;
            listChatRooms = listChatRooms.Skip(skipResult).Take(pageSize).ToList();
        }

        var chatRoomDto = listChatRooms.Select(chatRoomItem => new GetChatRoomDto()
        {
            ChatRoomId = chatRoomItem.ChatRoomId,
            NameRoom = chatRoomItem.NameRoom,
            CreateTime = chatRoomItem.CreateTime,
            UpdateTime = chatRoomItem.UpdateTime,
        }).ToList();

        return new ResponseDto()
        {
            Message = "Get chat rooms successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = chatRoomDto
        };
    }

    public async Task<ResponseDto> GetChatRoom(ClaimsPrincipal user, Guid userId)
    {
        var messages = await _unitOfWork.MessageRepository.GetAsync(m => m.UserId == userId.ToString());
        if (messages == null)
        {
            return new ResponseDto
            {
                Message = "No messages found for the user",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }
        var chatRoomId = messages.ChatRoomId;
        if (chatRoomId == null)
        {
            return new ResponseDto
            {
                Message = "Chat room id is null",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }
        
        var chatRoom = await _unitOfWork.ChatRoomRepository.GetById(chatRoomId.Value);
        if (chatRoom == null)
        {
            return new ResponseDto
            {
                Message = "Chat room not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var chatRoomDto = _mapper.Map<GetChatRoomDto>(chatRoom);

        return new ResponseDto
        {
            Message = "Chat room found successfully",
            Result = chatRoomDto,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> CreateChatRoom(ClaimsPrincipal user, CreateChatRoomDto createChatRoomDto)
    {
        ChatRoom newChatRoom = new ChatRoom()
        {
            ChatRoomId = new Guid(),
            NameRoom = createChatRoomDto.NameRoom,
            CreateTime = createChatRoomDto.CreateTime,
            UpdateTime = createChatRoomDto.UpdateTime,
        };

        await _unitOfWork.ChatRoomRepository.AddAsync(newChatRoom);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Chat room created successfully",
            Result = newChatRoom,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> UpdateChatRoom(ClaimsPrincipal user, UpdateChatRoomDto updateChatRoomDto)
    {
        var chatRoomId = await _unitOfWork.ChatRoomRepository.GetAsync(x => x.ChatRoomId == updateChatRoomDto.ChatRoomId);
        if (chatRoomId == null)
        {
            return new ResponseDto
            {
                Message = "Chat room not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        chatRoomId.ChatRoomId = updateChatRoomDto.ChatRoomId;
        chatRoomId.NameRoom = updateChatRoomDto.NameRoom;
        chatRoomId.CreateTime = updateChatRoomDto.CreateTime;
        chatRoomId.UpdateTime = updateChatRoomDto.UpdateTime;
        
        _unitOfWork.ChatRoomRepository.Update(chatRoomId);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Chat room updated successfully",
            Result = updateChatRoomDto,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> DeleteChatRoom(ClaimsPrincipal user, Guid chatRoomId)
    {
        var chatRooms = await _unitOfWork.ChatRoomRepository.GetAsync(x => x.ChatRoomId == chatRoomId);
        if (chatRooms == null)
        {
            return new ResponseDto
            {
                Message = "Chat room not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }
        
        _unitOfWork.ChatRoomRepository.Remove(chatRooms);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto()
        {
            Message = "Chat rooms delete successfully",
            Result = chatRooms,
            IsSuccess = true,
            StatusCode = 201
        };
    }
}