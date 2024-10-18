using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO.MemberRating;
using Ticket_Hub.Models.DTO;

namespace Ticket_Hub.Services.IServices
{
    public interface IMemberRatingService
    {
        Task<ResponseDto> GetMemberRatings
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

        Task<ResponseDto> GetMemberRating(ClaimsPrincipal user, Guid memberRatingId);

        Task<ResponseDto> CreateMemberRating(ClaimsPrincipal user, CreateMemberRatingDto createMemberRatingDto);

        Task<ResponseDto> UpdateMemberRating(ClaimsPrincipal user, UpdateMemberRatingDto updateMemberRatingDto);

        Task<ResponseDto> DeleteMemberRating(ClaimsPrincipal user, Guid memberRatingId);
    }
}
