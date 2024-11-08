using System.Security.Claims;
using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Transaction;
using Ticket_Hub.Models.DTO.Wallet;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GetTransactions(ClaimsPrincipal user, int pageNumber = 1, int pageSize = 10)
    {
        var allTransactions = await _unitOfWork.TransactionRepository.GetAllAsync();
        if (!allTransactions.Any())
        {
            return new ResponseDto
            {
                Message = "There are no transactions",
                IsSuccess = true,
                StatusCode = 404,
                Result = null
            };
        }

        var transactionList = allTransactions
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var transactionDtos = transactionList.Select(transaction => new GetTransactionDto()
        {
            TransactionId = transaction.TransactionId,
            WalletId = transaction.WalletId,
            Type = transaction.Type,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
        }).ToList();

        return new ResponseDto
        {
            Message = "Get transactions successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = transactionDtos
        };
    }

    public async Task<ResponseDto> GetTransaction(ClaimsPrincipal user, Guid transactionId)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetById(transactionId);
        if (transaction == null)
        {
            return new ResponseDto
            {
                Message = "Transaction not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var transactionDto = _mapper.Map<GetTransactionDto>(transaction);
        return new ResponseDto
        {
            Message = "Transaction found successfully",
            Result = transactionDto,
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> CreateTransaction(ClaimsPrincipal user, CreateTransactionDto createTransactionDto)
    {
        var newTransaction = new Transactions()
        {
            TransactionId = createTransactionDto.TransactionId,
            WalletId = createTransactionDto.WalletId,
            Type = createTransactionDto.Type,
            Amount = createTransactionDto.Amount,
            TransactionDate = createTransactionDto.TransactionDate
        };

        await _unitOfWork.TransactionRepository.AddAsync(newTransaction);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Transaction created successfully",
            Result = newTransaction,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> UpdateTransaction(ClaimsPrincipal user, UpdateTransactionDto updateTransactionDto)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetById(updateTransactionDto.TransactionId);
        if (transaction == null)
        {
            return new ResponseDto
            {
                Message = "Transaction not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        transaction.TransactionId = updateTransactionDto.TransactionId;
        transaction.WalletId = updateTransactionDto.WalletId;
        transaction.Type = updateTransactionDto.Type;
        transaction.Amount = updateTransactionDto.Amount;
        transaction.TransactionDate = updateTransactionDto.TransactionDate;

        _unitOfWork.TransactionRepository.Update(transaction);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Transaction updated successfully",
            Result = transaction,
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> DeleteTransaction(ClaimsPrincipal user, Guid transactionId)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetById(transactionId);
        if (transaction == null)
        {
            return new ResponseDto
            {
                Message = "Transaction not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        _unitOfWork.TransactionRepository.Update(transaction);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Transaction deleted successfully",
            Result = transaction,
            IsSuccess = true,
            StatusCode = 200
        };
    }
}