﻿using System.Security.Claims;
using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Event;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public EventService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GetEvents
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        IEnumerable<Event> allEvents = null!;

        // Lấy tất cả các sự kiện có trong database
        allEvents = await _unitOfWork.EventRepository.GetAllAsync();

        // Kiểm tra nếu danh sách events là null hoặc rỗng
        if (!allEvents.Any())
        {
            return new ResponseDto()
            {
                Message = "There are no events",
                IsSuccess = true,
                StatusCode = 404,
                Result = null
            };
        }

        var listEvents = allEvents.ToList();

        // Filter Query
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            switch (filterOn.Trim().ToLower())
            {
                case "eventname":
                    listEvents = listEvents.Where(x =>
                        x.EventName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                case "eventdate":
                    if (DateTime.TryParse(filterQuery, out DateTime filterDate))
                    {
                        listEvents = listEvents.Where(x =>
                                x.EventDate.Date >= filterDate.Date && x.EventDate.Date < filterDate.Date.AddDays(1))
                            .ToList();
                    }
                    break;
                default:
                    break;
            }
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            var sortParams = sortBy.Trim().ToLower().Split('_'); // Chia chuỗi sortBy theo ký tự '_'
            var sortField = sortParams[0]; // Tên cột cần sắp xếp
            var sortDirection = sortParams.Length > 1 ? sortParams[1] : "asc"; // Lấy hướng sắp xếp

            switch (sortField)
            {
                case "eventname":
                    listEvents = sortDirection == "desc"
                        ? listEvents.OrderByDescending(x => x.EventName).ToList()
                        : listEvents.OrderBy(x => x.EventName).ToList();
                    break;

                case "eventdate":
                    listEvents = sortDirection == "desc"
                        ? listEvents.OrderByDescending(x => x.EventDate).ToList()
                        : listEvents.OrderBy(x => x.EventDate).ToList();
                    break;

                default:
                    // Sắp xếp mặc định theo ngày gần nhất nếu không có cột phù hợp
                    listEvents = listEvents.OrderBy(x => x.EventDate).ToList();
                    break;
            }
        }
        else
        {
            // Sắp xếp mặc định theo ngày gần nhất nếu không có `sortBy`
            listEvents = listEvents.OrderBy(x => x.EventDate).ToList();
        }

        // Phân trang
        if (pageNumber > 0 && pageSize > 0)
        {
            var skipResult = (pageNumber - 1) * pageSize;
            listEvents = listEvents.Skip(skipResult).Take(pageSize).ToList();
        }

        // Chuyển đổi danh sách sự kiện thành DTO
        var eventDto = listEvents.Select(eventItem => new GetEventDto()
        {
            EventId = eventItem.EventId,
            EventName = eventItem.EventName,
            EventDate = eventItem.EventDate,
            // Thêm các thuộc tính khác nếu cần
        }).ToList();

        return new ResponseDto()
        {
            Message = "Get Events successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = eventDto
        };
    }

    public async Task<ResponseDto> GetEvent(ClaimsPrincipal user, Guid eventId)
    {
        var eventIDs = await _unitOfWork.EventRepository.GetById(eventId);
        if (eventIDs == null)
        {
            return new ResponseDto
            {
                Message = "Event not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var eventDto = _mapper.Map<GetEventDto>(eventIDs);

        return new ResponseDto
        {
            Message = "Event found successfully",
            Result = eventDto,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> CreateEvent(ClaimsPrincipal user, CreateEventDto createEventDto)
    {
        Event newEvent = new Event()
        {
            EventName = createEventDto.EventName,
            EventDescription = createEventDto.EventDescription,
            EventDate = createEventDto.EventDate,
            LocationId = createEventDto.LocationId,
            CreatedBy = user.Identity.Name,
            UpdatedBy = "",
            CreatedTime = DateTime.Now,
            UpdatedTime = null,
            Status = 1,
        };

        await _unitOfWork.EventRepository.AddAsync(newEvent);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Event created successfully",
            Result = newEvent,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> UpdateEvent(ClaimsPrincipal user, UpdateEventDto updateEventDto)
    {
        var eventId = await _unitOfWork.EventRepository.GetAsync(x => x.EventId == updateEventDto.EventId);
        if (eventId == null)
        {
            return new ResponseDto
            {
                Message = "Event not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        //update location
        eventId.EventName = updateEventDto.EventName;
        eventId.EventDescription = updateEventDto.EventDescription;
        eventId.EventDate = updateEventDto.EventDate;
        eventId.LocationId = updateEventDto.LocationId;
        eventId.UpdatedBy = user.Identity.Name;
        eventId.UpdatedTime = DateTime.UtcNow;


        //save changes
        _unitOfWork.EventRepository.Update(eventId);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Event updated successfully",
            Result = eventId,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> DeleteEvent(ClaimsPrincipal user, Guid eventId)
    {
        var events = await _unitOfWork.EventRepository.GetAsync(x => x.EventId == eventId);
        if (events == null)
        {
            return new ResponseDto
            {
                Message = "Location not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        events.Status = 0;
        events.UpdatedBy = user.Identity.Name;
        events.UpdatedTime = DateTime.UtcNow;

        //save changes
        _unitOfWork.EventRepository.Update(events);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto()
        {
            Message = "Events delete successfully",
            Result = events,
            IsSuccess = true,
            StatusCode = 201
        };
    }
}