using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.MemberRating;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberRatingController : ControllerBase
    {
        private readonly IMemberRatingService _memberRatingService;

        public MemberRatingController(IMemberRatingService memberRatingService)
        {
            _memberRatingService = memberRatingService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetMemberRatings
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 0
        )
        {
            var responseDto = await _memberRatingService.GetMemberRatings(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{memberRatingId}")]
        public async Task<ActionResult<ResponseDto>> GetMemberRating
        (
            [FromRoute] Guid memberRatingId
        )
        {
            var responseDto = await _memberRatingService.GetMemberRating(User, memberRatingId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateMemberRating
        (
            [FromBody] CreateMemberRatingDto createMemberRatingDto
        )
        {
            var responseDto = await _memberRatingService.CreateMemberRating(User, createMemberRatingDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateMemberRating
        (
            [FromBody] UpdateMemberRatingDto updateMemberRatingDto
        )
        {
            var responseDto = await _memberRatingService.UpdateMemberRating(User, updateMemberRatingDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{memberRatingId}")]
        public async Task<ActionResult<ResponseDto>> DeleteMemberRating
        (
            [FromRoute] Guid memberRatingId
        )
        {
            var responseDto = await _memberRatingService.DeleteMemberRating(User, memberRatingId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
