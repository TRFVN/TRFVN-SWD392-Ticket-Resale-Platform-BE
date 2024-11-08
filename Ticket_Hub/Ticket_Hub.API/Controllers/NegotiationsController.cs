using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Negotiation;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NegotiationsController : Controller
{
    private readonly INegotiationsService _negotiationsService;
    
    public NegotiationsController(INegotiationsService negotiationsService)
    {
        _negotiationsService = negotiationsService;
    }
    
    // Phương thức tạo đàm phán
    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> CreateNegotiations
    (
        [FromBody] CreateNegotiationsDto createNegotiationsDto
    )
    {
        var responseDto = await _negotiationsService.CreateNegotiations(User, createNegotiationsDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }
    
    // Phương thức chấp nhận đàm phán
    [HttpPost("accept")]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> AcceptNegotiations
    (
        [FromBody] NegotiationDto negotiationDto
    )
    {
        var responseDto = await _negotiationsService.AcceptNegotiations(User, negotiationDto.NegotiationId);
        return StatusCode(responseDto.StatusCode, responseDto);
    }
}