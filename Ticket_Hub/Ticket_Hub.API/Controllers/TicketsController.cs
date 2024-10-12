using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Ticket;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetTickets
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 0
        )
        {
            var responseDto = await _ticketService.GetTickets
            (
                User,
                filterOn,
                filterQuery,
                sortBy,
                pageNumber,
                pageSize
            );
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateLevel
        (
            [FromBody] CreateTicketDto createLevelDto
        )
        {
            var responseDto = await _ticketService.CreateTicket(User, createLevelDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateLevel
        (
            [FromBody] UpdateTicketDto updateLevelDto
        )
        {
            var responseDto = await _ticketService.UpdateTicket(User, updateLevelDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("{ticketId}")]
        public async Task<ActionResult<ResponseDto>> DeleteLevel
        (
            [FromRoute] Guid ticketId
        )
        {
            var responseDto = await _ticketService.DeleteTicket(User, ticketId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}