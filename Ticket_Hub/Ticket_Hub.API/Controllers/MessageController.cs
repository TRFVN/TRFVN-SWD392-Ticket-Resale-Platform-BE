using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ticket_Hub.Models.DTO.Message;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Models.DTO.Hubs;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<NotificationHub> _hubContext; // SignalR Hub for notifications

        public MessageController(IMessageService messageService, IHubContext<NotificationHub> hubContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetMessages
    (
        [FromQuery] string? filterOn,
        [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
        {
            var responseDto = await _messageService.GetMessages(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{messageId}")]
        public async Task<ActionResult<ResponseDto>> GetMessage
        (
            [FromRoute] Guid messageId
        )
        {
            var responseDto = await _messageService.GetMessage(User, messageId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateMessage
        (
            [FromBody] CreateMessageDto createMessageDto
        )
        {
            var responseDto = await _messageService.CreateMessage(User, createMessageDto);

            if (responseDto.IsSuccess)
            {
                // Send a real-time notification to all connected clients
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", "A new message was created");
            }

            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageDto messageDto)
        {
            // Gửi tin nhắn tới tất cả các client kết nối
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", messageDto.UserId, messageDto.MessageContent);

            return Ok(new { Message = "Message sent successfully" });
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateMessage
        (
            [FromBody] UpdateMessageDto updateMessageDto
        )
        {
            var responseDto = await _messageService.UpdateMessage(User, updateMessageDto);

            if (responseDto.IsSuccess)
            {
                // Send a real-time notification to all connected clients
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", "A message was updated");
            }

            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{messageId}")]
        public async Task<ActionResult<ResponseDto>> DeleteMessage
    (
        [FromRoute] Guid messageId
    )
        {
            var responseDto = await _messageService.DeleteMessage(User, messageId);

            if (responseDto.IsSuccess)
            {
                // Send a real-time notification to all connected clients
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", "A message was deleted");
            }

            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
