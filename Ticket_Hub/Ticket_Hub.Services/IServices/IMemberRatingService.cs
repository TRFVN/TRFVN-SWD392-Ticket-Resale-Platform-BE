using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Services.IServices
{
    public interface IMemberRatingService
    {
        Task<IEnumerable<MemberRating>> GetAllMemberRatingsAsync();
        Task<MemberRating?> GetMemberRatingByIdAsync(Guid id);
        Task<ResponseDto> CreateMemberRatingAsync(MemberRating memberRating);
        Task UpdateMemberRatingAsync(MemberRating memberRating);
        Task<ResponseDto> DeleteMemberRatingAsync(Guid id);
    }
}
