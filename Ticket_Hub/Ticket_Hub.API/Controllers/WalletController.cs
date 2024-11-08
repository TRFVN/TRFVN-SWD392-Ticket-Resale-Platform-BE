using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Feedback;
using Ticket_Hub.Models.DTO.Wallet;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetWallets
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 0
        )
        {
            var responseDto = await _walletService.GetWallets(User, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{walletId}")]
        public async Task<ActionResult<ResponseDto>> GetFeedback
        (
            [FromRoute] Guid feedbackId
        )
        {
            var responseDto = await _walletService.GetWallet(User, feedbackId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateFeedback
        (
            [FromBody] CreateWalletDto createWalletDto
        )
        {
            var responseDto = await _walletService.CreateWallet(User, createWalletDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateFeedback
        (
            [FromBody] UpdateWalletDto updateWalletDto
        )
        {
            var responseDto = await _walletService.UpdateWallet(User, updateWalletDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{walletId}")]
        public async Task<ActionResult<ResponseDto>> DeleteFeedback
        (
            [FromRoute] Guid walletId
        )
        {
            var responseDto = await _walletService.DeleteWallet(User, walletId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}