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
        
        /// <summary>
        /// Get all tickets
        /// </summary>
        /// <param name="filterOn"></param>
        /// <param name="filterQuery"></param>
        /// <param name="sortBy"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetTickets
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
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
        
        /// <summary>
        /// Get ticket by ticketId
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{ticketId}")]
        public async Task<ActionResult<ResponseDto>> GetTicket
        (
            [FromRoute] Guid ticketId
        )
        {
            var responseDto = await _ticketService.GetTicket(User, ticketId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Create a new ticket
        /// </summary>
        /// <param name="createLevelDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateTicket
        (
            [FromBody] CreateTicketDto createLevelDto
        )
        {
            var responseDto = await _ticketService.CreateTicket(User, createLevelDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Update ticket
        /// </summary>
        /// <param name="updateLevelDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateTicket
        (
            [FromBody] UpdateTicketDto updateLevelDto
        )
        {
            var responseDto = await _ticketService.UpdateTicket(User, updateLevelDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        /// <summary>
        /// Delete ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpDelete("{ticketId}")]
        public async Task<ActionResult<ResponseDto>> DeleteTicket
        (
            [FromRoute] Guid ticketId
        )
        {
            var responseDto = await _ticketService.DeleteTicket(User, ticketId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}