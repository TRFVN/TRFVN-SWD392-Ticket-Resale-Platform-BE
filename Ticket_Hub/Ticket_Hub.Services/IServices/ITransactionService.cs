using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Transaction;

namespace Ticket_Hub.Services.IServices;

public interface ITransactionService
{
    Task<ResponseDto> GetTransactions(ClaimsPrincipal user, int pageNumber = 1, int pageSize = 10);
    Task<ResponseDto> GetTransaction(ClaimsPrincipal user, Guid transactionId);
    Task<ResponseDto> CreateTransaction(ClaimsPrincipal user, CreateTransactionDto createTransactionDto);
    Task<ResponseDto> UpdateTransaction(ClaimsPrincipal user, UpdateTransactionDto updateTransactionDto);
    Task<ResponseDto> DeleteTransaction(ClaimsPrincipal user, Guid transactionId);
}