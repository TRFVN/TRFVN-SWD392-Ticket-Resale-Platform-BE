﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ticket_Hub.Models.DTO.Message;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.ChatRoom;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Models.DTO.Hubs;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<NotificationHub> _hubContext; 

        public MessageController(IMessageService messageService, IHubContext<NotificationHub> hubContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetMessages([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var responseDto = await _messageService.GetMessages(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{chatRoomId}")]
        public async Task<ActionResult<ResponseDto>> GetMessage([FromRoute] Guid chatRoomId)
        {
            var responseDto = await _messageService.GetMessage(User, chatRoomId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateMessage([FromBody] CreateMessageDto createMessageDto)
        {
            var responseDto = await _messageService.CreateMessage(User, createMessageDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateMessage([FromBody] UpdateMessageDto updateMessageDto)
        {
            var responseDto = await _messageService.UpdateMessage(User, updateMessageDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{messageId}")]
        public async Task<ActionResult<ResponseDto>> DeleteMessage([FromRoute] Guid messageId)
        {
            var responseDto = await _messageService.DeleteMessage(User, messageId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
