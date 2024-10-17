using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Feedback;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetFeedbacks
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 0
        )
        {
            var responseDto = await _feedbackService.GetFeedbacks(User, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{feedbackId}")]
        public async Task<ActionResult<ResponseDto>> GetFeedback
        (
            [FromRoute] Guid feedbackId
        )
        {
            var responseDto = await _feedbackService.GetFeedback(User, feedbackId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateFeedback
        (
            [FromBody] CreateFeedbackDto createFeedbackDto
        )
        {
            var responseDto = await _feedbackService.CreateFeedback(User, createFeedbackDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateFeedback
        (
            [FromBody] UpdateFeedbackDto updateFeedbackDto
        )
        {
            var responseDto = await _feedbackService.UpdateFeedback(User, updateFeedbackDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{feedbackId}")]
        public async Task<ActionResult<ResponseDto>> DeleteFeedback
        (
            [FromRoute] Guid feedbackId
        )
        {
            var responseDto = await _feedbackService.DeleteFeedback(User, feedbackId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
