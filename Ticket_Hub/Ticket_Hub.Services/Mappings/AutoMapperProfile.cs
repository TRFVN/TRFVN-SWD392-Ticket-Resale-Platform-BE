using AutoMapper;
using Ticket_Hub.Models.DTO.Category;
using Ticket_Hub.Models.DTO.ChatRoom;
using Ticket_Hub.Models.DTO.Event;
using Ticket_Hub.Models.DTO.Feedback;

using Ticket_Hub.Models.DTO.Message;

using Ticket_Hub.Models.DTO.Ticket;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Services.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Ticket, GetTicketDto>().ReverseMap();

        CreateMap<Event, GetEventDto>().ReverseMap();
        CreateMap<Category, GetCategoryDto>().ReverseMap();
        CreateMap<Category, GetCategoryByIdDto>()
            .ForMember(dest =>dest.Id, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : null))  
            .ForMember(dest => dest.SubcategoryNames, opt => opt.MapFrom(src => src.SubCategories.Select(sub => sub.SubCategories).ToList())); 
        CreateMap<Feedback, GetFeedbackDto>().ReverseMap();

        CreateMap<Message, GetMessageDto>().ReverseMap();
        CreateMap<ChatRoom, GetChatRoomDto>().ReverseMap();
    }
}