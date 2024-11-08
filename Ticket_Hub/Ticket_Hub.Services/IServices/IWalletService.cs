using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Wallet;

namespace Ticket_Hub.Services.IServices;

public interface IWalletService
{
    Task<ResponseDto> GetWallets(ClaimsPrincipal user, int pageNumber = 1, int pageSize = 10);
    Task<ResponseDto> GetWallet(ClaimsPrincipal user, Guid walletId);
    Task<ResponseDto> CreateWallet(ClaimsPrincipal user, CreateWalletDto createWalletDto);
    Task<ResponseDto> UpdateWallet(ClaimsPrincipal user, UpdateWalletDto updateWalletDto);
    Task<ResponseDto> DeleteWallet(ClaimsPrincipal user, Guid walletId);
}