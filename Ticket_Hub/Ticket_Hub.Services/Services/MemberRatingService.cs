using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services
{
    public class MemberRatingService : IMemberRatingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberRatingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseDto> CreateMemberRatingAsync(MemberRating memberRating)
        {
            await _unitOfWork.MemberRating.AddAsync(memberRating);
            await _unitOfWork.SaveAsync();
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Member rating created successfully."
            };
        }

        public async Task<ResponseDto> DeleteMemberRatingAsync(Guid id)
        {
            var memberRating = await _unitOfWork.MemberRating.GetByIdAsync(id);
            if (memberRating != null)
            {
                _unitOfWork.MemberRating.Remove(memberRating);
                await _unitOfWork.SaveAsync();
            }
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Member rating deleted successfully."
            };
        }

        public async Task<IEnumerable<MemberRating>> GetAllMemberRatingsAsync()
        {
            return await _unitOfWork.MemberRating.GetAllAsync();
        }

        public async Task<MemberRating?> GetMemberRatingByIdAsync(Guid id)
        {
            return await _unitOfWork.MemberRating.GetByIdAsync(id);
        }

        public async Task UpdateMemberRatingAsync(MemberRating memberRating)
        {
            _unitOfWork.MemberRating.Update(memberRating);
            await _unitOfWork.SaveAsync();
        }
    }
}
