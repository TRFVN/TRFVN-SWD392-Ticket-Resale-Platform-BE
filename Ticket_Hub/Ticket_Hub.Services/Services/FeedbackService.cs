using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO.Feedback;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Models.DTO.MemberRating;

namespace Ticket_Hub.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetFeedbacks(ClaimsPrincipal user, int pageNumber = 1, int pageSize = 10)
        {
            var allFeedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync();
            if (!allFeedbacks.Any())
            {
                return new ResponseDto
                {
                    Message = "There are no feedbacks",
                    IsSuccess = true,
                    StatusCode = 404,
                    Result = null
                };
            }

            var feedbackList = allFeedbacks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var feedbackDtos = feedbackList.Select(feedback => new GetFeedbackDto
            {
                FeedbackId = feedback.FeedbackId,
                UserId = feedback.UserId,
                Content = feedback.Content,
                // Thêm các thuộc tính khác nếu cần
            }).ToList();

            return new ResponseDto
            {
                Message = "Get Feedbacks successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = feedbackDtos
            };
        }

        public async Task<ResponseDto> GetFeedback(ClaimsPrincipal user, Guid feedbackId)
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

            var feedbackDto = _mapper.Map<GetFeedbackDto>(feedback);
            return new ResponseDto
            {
                Message = "Feedback found successfully",
                Result = feedbackDto,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> CreateFeedback(ClaimsPrincipal user, CreateFeedbackDto createFeedbackDto)
        {
            var newFeedback = new Feedback
            {
                UserId = createFeedbackDto.UserId,
                Content = createFeedbackDto.Content,
                CreatedBy = user.Identity.Name,
                CreatedTime = DateTime.Now,
                UpdatedBy = "",
                UpdatedTime = null,
                Status = 1,
            };

            await _unitOfWork.FeedbackRepository.AddAsync(newFeedback);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Feedback created successfully",
                Result = newFeedback,
                IsSuccess = true,
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> UpdateFeedback(ClaimsPrincipal user, UpdateFeedbackDto updateFeedbackDto)
        {
            var feedback = await _unitOfWork.FeedbackRepository.GetById(updateFeedbackDto.FeedbackId);
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

            // Cập nhật thông tin feedback
            feedback.Content = updateFeedbackDto.Content;
            feedback.UpdatedBy = user.Identity.Name;
            feedback.UpdatedTime = DateTime.UtcNow;

            _unitOfWork.FeedbackRepository.Update(feedback);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Feedback updated successfully",
                Result = feedback,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> DeleteFeedback(ClaimsPrincipal user, Guid feedbackId)
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

            feedback.Status = 0; // Hoặc tùy thuộc vào cách bạn quản lý trạng thái
            feedback.UpdatedBy = user.Identity.Name;
            feedback.UpdatedTime = DateTime.UtcNow;

            _unitOfWork.FeedbackRepository.Update(feedback);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Feedback deleted successfully",
                Result = feedback,
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
