using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.MemberRating;
using Ticket_Hub.Models.Models;
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

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] MemberRatingDto memberRatingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var memberRating = new MemberRating
                {
                    UserId = memberRatingDto.UserId,
                    Rate = memberRatingDto.Rate
                };
                var result = await _memberRatingService.CreateMemberRatingAsync(memberRating);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto>> GetById(Guid id)
        {
            var response = await _memberRatingService.GetMemberRatingByIdAsync(id);
            if (response == null)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Member rating not found."
                });
            }

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                Message = "Member rating retrieved successfully.",
                Result = response
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
            var response = await _memberRatingService.DeleteMemberRatingAsync(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
