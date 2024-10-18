﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.DataAccess.Repository;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Message;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateMessage(ClaimsPrincipal user, CreateMessageDto createMessageDto)
        {
            Message newMessage = new Message()
            {
                MessageContent = createMessageDto.MessageContent,
                UserId = createMessageDto.UserId,
                CreateTime = DateTime.UtcNow
            };

            await _unitOfWork.MessageRepository.AddAsync(newMessage);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Message created successfully",
                Result = newMessage,
                IsSuccess = true,
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> DeleteMessage(ClaimsPrincipal user, Guid messageId)
        {
            var message = await _unitOfWork.MessageRepository.GetAsync(x => x.MessageId == messageId);
            if (message == null)
            {
                return new ResponseDto
                {
                    Message = "Message not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            _unitOfWork.MessageRepository.Remove(message);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Message deleted successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetMessage(ClaimsPrincipal user, Guid messageId)
        {
            var message = await _unitOfWork.MessageRepository.GetById(messageId);
            if (message == null)
            {
                return new ResponseDto
                {
                    Message = "Message not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var messageDto = _mapper.Map<GetMessageDto>(message);
            return new ResponseDto
            {
                Message = "Message found successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = messageDto
            };
        }

        public async Task<ResponseDto> GetMessages(ClaimsPrincipal user, string? filterOn, string? filterQuery, string? sortBy, int pageNumber = 0, int pageSize = 0)
        {
            var allMessages = await _unitOfWork.MessageRepository.GetAllAsync();

            if (!allMessages.Any())
            {
                return new ResponseDto
                {
                    Message = "There are no messages",
                    IsSuccess = true,
                    StatusCode = 404,
                    Result = null
                };
            }

            var listMessages = allMessages.ToList();

            // Filter
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "userid":
                        listMessages = listMessages
                            .Where(x => x.UserId.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase))
                            .ToList();
                        break;
                    default:
                        break;
                }
            }

            // Sort
            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortParams = sortBy.Trim().ToLower().Split('_');
                var sortField = sortParams[0];
                var sortDirection = sortParams.Length > 1 ? sortParams[1] : "asc";

                switch (sortField)
                {
                    case "createtime":
                        listMessages = sortDirection == "desc"
                            ? listMessages.OrderByDescending(x => x.CreateTime).ToList()
                            : listMessages.OrderBy(x => x.CreateTime).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                listMessages = listMessages.OrderBy(x => x.CreateTime).ToList();
            }

            // Phân trang
            var skipResult = (pageNumber - 1) * pageSize;
            listMessages = listMessages.Skip(skipResult).Take(pageSize).ToList();

            // Map to DTO
            var messageDtos = listMessages.Select(msg => _mapper.Map<GetMessageDto>(msg)).ToList();

            return new ResponseDto
            {
                Message = "Get messages successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = messageDtos
            };
        }

        public async Task<ResponseDto> UpdateMessage(ClaimsPrincipal user, UpdateMessageDto updateMessageDto)
        {
            var message = await _unitOfWork.MessageRepository.GetAsync(x => x.MessageId == updateMessageDto.MessageId);
            if (message == null)
            {
                return new ResponseDto
                {
                    Message = "Message not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            message.MessageContent = updateMessageDto.MessageContent;
            message.CreateTime = DateTime.UtcNow;

            _unitOfWork.MessageRepository.Update(message);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Message updated successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = message
            };
        }
    }
}