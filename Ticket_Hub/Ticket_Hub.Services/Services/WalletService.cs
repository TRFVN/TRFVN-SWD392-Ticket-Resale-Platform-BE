using System.Security.Claims;
using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Wallet;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class WalletService : IWalletService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GetWallets(ClaimsPrincipal user, int pageNumber = 1, int pageSize = 10)
    {
        var allWallets = await _unitOfWork.WalletRepository.GetAllAsync();
        if (!allWallets.Any())
        {
            return new ResponseDto
            {
                Message = "There are no wallets",
                IsSuccess = true,
                StatusCode = 404,
                Result = null
            };
        }

        var walletList = allWallets
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var walletDtos = walletList.Select(wallet => new GetWalletDto
        {
            WalletId = wallet.WalletId,
            TotalBalance = wallet.TotalBalance,
            PayoutBalance = wallet.PayoutBalance,
            UpdateTime = wallet.UpdateTime,
            UserId = wallet.UserId,
        }).ToList();

        return new ResponseDto
        {
            Message = "Get wallets successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = walletDtos
        };
    }

    public async Task<ResponseDto> GetWallet(ClaimsPrincipal user, Guid feedbackId)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetById(feedbackId);
        if (feedback == null)
        {
            return new ResponseDto
            {
                Message = "Feedback not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var feedbackDto = _mapper.Map<GetWalletDto>(feedback);
        return new ResponseDto
        {
            Message = "Feedback found successfully",
            Result = feedbackDto,
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> CreateWallet(ClaimsPrincipal user, CreateWalletDto createWalletDto)
    {
        var newWallet = new Wallet()
        {
            WalletId = createWalletDto.WalletId,
            TotalBalance = createWalletDto.TotalBalance,
            PayoutBalance = createWalletDto.PayoutBalance,
            UpdateTime = DateTime.Now,
            UserId = createWalletDto.UserId
        };

        await _unitOfWork.WalletRepository.AddAsync(newWallet);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Wallet created successfully",
            Result = newWallet,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> UpdateWallet(ClaimsPrincipal user, UpdateWalletDto updateWalletDto)
    {
        var wallet = await _unitOfWork.WalletRepository.GetById(updateWalletDto.WalletId);
        if (wallet == null)
        {
            return new ResponseDto
            {
                Message = "Wallet not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        // Cập nhật thông tin feedback
        wallet.TotalBalance = updateWalletDto.TotalBalance;
        wallet.PayoutBalance = updateWalletDto.PayoutBalance;
        wallet.UpdateTime = DateTime.UtcNow;
        wallet.UserId = updateWalletDto.UserId;

        _unitOfWork.WalletRepository.Update(wallet);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Wallet updated successfully",
            Result = wallet,
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> DeleteWallet(ClaimsPrincipal user, Guid walletId)
    {
        var wallet = await _unitOfWork.WalletRepository.GetById(walletId);
        if (wallet == null)
        {
            return new ResponseDto
            {
                Message = "Wallet not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        wallet.TotalBalance = 0;
        wallet.TotalBalance = 0;
        wallet.UpdateTime = DateTime.UtcNow;

        _unitOfWork.WalletRepository.Update(wallet);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Wallet deleted successfully",
            Result = wallet,
            IsSuccess = true,
            StatusCode = 200
        };
    }
}