using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO.MemberRating;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services
{
    public class MemberRatingService : IMemberRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public MemberRatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetMemberRatings
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 10
    )
        {
            IEnumerable<MemberRating> allMemberRatings = null!;
            // Lấy tất cả các đánh giá thành viên có trong database
            allMemberRatings = await _unitOfWork.MemberRatingRepository.GetAllAsync();

            if (!allMemberRatings.Any())
            {
                return new ResponseDto()
                {
                    Message = "There are no member ratings",
                    IsSuccess = true,
                    StatusCode = 404,
                    Result = null
                };
            }

            var listMemberRatings = allMemberRatings.ToList();

            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "ratingvalue":
                        if (decimal.TryParse(filterQuery, out decimal filterValue))
                        {
                            listMemberRatings = listMemberRatings
                                .Where(x => x.Rate == filterValue)
                                .ToList();
                        }
                        break;
                    case "userid":
                        listMemberRatings = listMemberRatings
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
                    case "ratingvalue":
                        listMemberRatings = sortDirection == "desc"
                            ? listMemberRatings.OrderByDescending(x => x.Rate).ToList()
                            : listMemberRatings.OrderBy(x => x.Rate).ToList();
                        break;
                    case "createdtime":
                        listMemberRatings = sortDirection == "desc"
                            ? listMemberRatings.OrderByDescending(x => x.CreatedTime).ToList()
                            : listMemberRatings.OrderBy(x => x.CreatedTime).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                listMemberRatings = listMemberRatings.OrderBy(x => x.CreatedTime).ToList();
            }

            // Phân trang
            if (pageNumber > 0 && pageSize > 0)
            {
                var skipResult = (pageNumber - 1) * pageSize;
                listMemberRatings = listMemberRatings.Skip(skipResult).Take(pageSize).ToList();
            }

            // Chuyển đổi danh sách đánh giá thành DTO
            var memberRatingDto = listMemberRatings.Select(ratingItem => new GetMemberRatingDto()
            {
                MemberRatingId = ratingItem.MemberRatingId,
                UserId = ratingItem.UserId,  // Thêm thuộc tính UserId
                Rate = ratingItem.Rate
                // Thêm các thuộc tính khác nếu cần
            }).ToList();

            return new ResponseDto()
            {
                Message = "Get member ratings successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = memberRatingDto
            };
        }

        public async Task<ResponseDto> GetMemberRating(ClaimsPrincipal user, Guid memberRatingId)
        {
            var memberRating = await _unitOfWork.MemberRatingRepository.GetById(memberRatingId);
            if (memberRating == null)
            {
                return new ResponseDto
                {
                    Message = "Member rating not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var memberRatingDto = _mapper.Map<GetMemberRatingDto>(memberRating);
            return new ResponseDto
            {
                Message = "Member rating found successfully",
                Result = memberRatingDto,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> CreateMemberRating(ClaimsPrincipal user, CreateMemberRatingDto createMemberRatingDto)
        {
            MemberRating newMemberRating = new MemberRating()
            {
                UserId = createMemberRatingDto.UserId.ToString(),
                Rate = createMemberRatingDto.Rate,
                CreatedBy = user.Identity.Name,
                CreatedTime = DateTime.UtcNow,
                Status = 1 // Giả sử bạn có trường trạng thái
            };

            await _unitOfWork.MemberRatingRepository.AddAsync(newMemberRating);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Member rating created successfully",
                Result = newMemberRating,
                IsSuccess = true,
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> UpdateMemberRating(ClaimsPrincipal user, UpdateMemberRatingDto updateMemberRatingDto)
        {
            var memberRating = await _unitOfWork.MemberRatingRepository.GetAsync(x => x.MemberRatingId == updateMemberRatingDto.MemberRatingId);
            if (memberRating == null)
            {
                return new ResponseDto
                {
                    Message = "Member rating not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            // Cập nhật thông tin thành viên
            memberRating.Rate = updateMemberRatingDto.Rate;
            memberRating.UpdatedBy = user.Identity.Name;
            memberRating.UpdatedTime = DateTime.UtcNow;

            // Lưu thay đổi
            _unitOfWork.MemberRatingRepository.Update(memberRating);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Member rating updated successfully",
                Result = memberRating,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> DeleteMemberRating(ClaimsPrincipal user, Guid memberRatingId)
        {
            var memberRating = await _unitOfWork.MemberRatingRepository.GetAsync(x => x.MemberRatingId == memberRatingId);
            if (memberRating == null)
            {
                return new ResponseDto
                {
                    Message = "Member rating not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            memberRating.Status = 0; // Giả sử trạng thái 0 là đã xóa
            memberRating.UpdatedBy = user.Identity.Name;
            memberRating.UpdatedTime = DateTime.UtcNow;

            // Lưu thay đổi
            _unitOfWork.MemberRatingRepository.Update(memberRating);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Message = "Member rating deleted successfully",
                Result = memberRating,
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
