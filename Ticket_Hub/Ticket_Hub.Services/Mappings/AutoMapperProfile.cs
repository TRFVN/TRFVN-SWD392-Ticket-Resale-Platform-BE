using AutoMapper;
using Ticket_Hub.Models.DTO.Event;
using Ticket_Hub.Models.DTO.Location;
using Ticket_Hub.Models.DTO.Ticket;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Services.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Ticket, GetTicketDto>().ReverseMap();
        CreateMap<Location, GetLocationDto>().ReverseMap();
        CreateMap<Event, GetEventDto>().ReverseMap();
    }
}