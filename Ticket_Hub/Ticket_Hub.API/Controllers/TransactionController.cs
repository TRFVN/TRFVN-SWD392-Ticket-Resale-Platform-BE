using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Transaction;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers

{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetTransactions
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 0
        )
        {
            var responseDto = await _transactionService.GetTransactions(User, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{transactionId}")]
        public async Task<ActionResult<ResponseDto>> GetTransaction
        (
            [FromRoute] Guid transactionId
        )
        {
            var responseDto = await _transactionService.GetTransaction(User, transactionId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateTransaction
        (
            [FromBody] CreateTransactionDto createTransactionDto
        )
        {
            var responseDto = await _transactionService.CreateTransaction(User, createTransactionDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateTransaction
        (
            [FromBody] UpdateTransactionDto updateTransactionDto
        )
        {
            var responseDto = await _transactionService.UpdateTransaction(User, updateTransactionDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{transactionId}")]
        public async Task<ActionResult<ResponseDto>> DeleteTransaction
        (
            [FromRoute] Guid transactionId
        )
        {
            var responseDto = await _transactionService.DeleteTransaction(User, transactionId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}