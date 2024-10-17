using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO.Feedback;
using Ticket_Hub.Models.DTO;

namespace Ticket_Hub.Services.IServices
{
    public interface IFeedbackService
    {
        Task<ResponseDto> GetFeedbacks(ClaimsPrincipal user, int pageNumber = 1, int pageSize = 10);
        Task<ResponseDto> GetFeedback(ClaimsPrincipal user, Guid feedbackId);
        Task<ResponseDto> CreateFeedback(ClaimsPrincipal user, CreateFeedbackDto createFeedbackDto);
        Task<ResponseDto> UpdateFeedback(ClaimsPrincipal user, UpdateFeedbackDto updateFeedbackDto);
        Task<ResponseDto> DeleteFeedback(ClaimsPrincipal user, Guid feedbackId);
    }
}
