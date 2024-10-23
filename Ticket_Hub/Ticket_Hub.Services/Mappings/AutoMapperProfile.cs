using AutoMapper;
using Ticket_Hub.Models.DTO.CartDetail;
using Ticket_Hub.Models.DTO.CartHeader;
using Ticket_Hub.Models.DTO.Category;
using Ticket_Hub.Models.DTO.Event;
using Ticket_Hub.Models.DTO.Favourite;
using Ticket_Hub.Models.DTO.Feedback;
using Ticket_Hub.Models.DTO.Location;
using Ticket_Hub.Models.DTO.MemberRating;
using Ticket_Hub.Models.DTO.Message;
using Ticket_Hub.Models.DTO.SubCategory;
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
        CreateMap<Category, GetCategoryDto>().ReverseMap();
        CreateMap<SubCategory, GetSubCategoryDto>().ReverseMap();
        CreateMap<MemberRating, GetMemberRatingDto>().ReverseMap();
        CreateMap<Feedback, GetFeedbackDto>().ReverseMap();
        CreateMap<Favourite, GetFavouriteDto>().ReverseMap();
        CreateMap<Message, GetMessageDto>().ReverseMap();
        CreateMap<CartHeader, GetCartHeaderDto>().ReverseMap();
        CreateMap<CartDetail, GetCartDetailDto>().ReverseMap();
    }
}