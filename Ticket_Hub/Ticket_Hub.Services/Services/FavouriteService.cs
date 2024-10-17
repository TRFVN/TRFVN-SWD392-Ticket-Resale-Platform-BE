using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Favourite;
using Ticket_Hub.Models.DTO.Feedback;
using Ticket_Hub.Models.DTO.MemberRating;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FavouriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateFavourite(ClaimsPrincipal user, CreateFavouriteDto createFavouriteDto)
        {
            var newFavourite = new Favourite
            {
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                TicketId = createFavouriteDto.TicketId,
                CreatedBy = user.Identity.Name,
                CreatedTime = DateTime.UtcNow,
                UpdatedBy = "",
                UpdatedTime = null,
                Status = 1,
            };

            await _unitOfWork.FavoriteRepository.AddAsync(newFavourite);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Favourite created successfully",
                Result = newFavourite,
                IsSuccess = true,
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> DeleteFavourite(ClaimsPrincipal user, Guid favouriteId)
        {
            var favourite = await _unitOfWork.FeedbackRepository.GetById(favouriteId);
            if (favourite == null)
            {
                return new ResponseDto
                {
                    Message = "Feedback not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            favourite.Status = 0; // Hoặc tùy thuộc vào cách bạn quản lý trạng thái
            favourite.UpdatedBy = user.Identity.Name;
            favourite.UpdatedTime = DateTime.UtcNow;

            _unitOfWork.FeedbackRepository.Update(favourite);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Feedback deleted successfully",
                Result = favourite,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetFavourite(ClaimsPrincipal user, Guid favouriteId)
        {
            var favourite = await _unitOfWork.FeedbackRepository.GetById(favouriteId);
            if (favourite == null)
            {
                return new ResponseDto
                {
                    Message = "Favourite not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var favouriteDto = _mapper.Map<GetFeedbackDto>(favourite);
            return new ResponseDto
            {
                Message = "Favourite found successfully",
                Result = favouriteDto,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetFavourites(ClaimsPrincipal user, string? filterOn, string? filterQuery, string? sortBy, int pageNumber = 0, int pageSize = 0)
        {
            IEnumerable<Favourite> allFavourites = null!;

            allFavourites = await _unitOfWork.FavoriteRepository.GetAllAsync();

            if (!allFavourites.Any())
            {
                return new ResponseDto()
                {
                    Message = "There are no favourites",
                    IsSuccess = true,
                    StatusCode = 404,
                    Result = null
                };
            }

            var listFavourites = allFavourites.ToList();


            // Sort
            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortParams = sortBy.Trim().ToLower().Split('_');
                var sortField = sortParams[0];
                var sortDirection = sortParams.Length > 1 ? sortParams[1] : "asc";

                switch (sortField)
                {
                    case "ticket":
                        listFavourites = sortDirection == "desc"
                            ? listFavourites.OrderByDescending(x => x.TicketId).ToList()
                            : listFavourites.OrderBy(x => x.TicketId).ToList();
                        break;
                    case "createdtime":
                        listFavourites = sortDirection == "desc"
                            ? listFavourites.OrderByDescending(x => x.CreatedTime).ToList()
                            : listFavourites.OrderBy(x => x.CreatedTime).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                listFavourites = listFavourites.OrderBy(x => x.CreatedTime).ToList();
            }

            // Phân trang
            if (pageNumber > 0 && pageSize > 0)
            {
                var skipResult = (pageNumber - 1) * pageSize;
                listFavourites = listFavourites.Skip(skipResult).Take(pageSize).ToList();
            }

            // Chuyển đổi danh sách đánh giá thành DTO
            var favouritesDto = listFavourites.Select(favouriteItem => new GetFavouriteDto()
            {
                FavouriteId = favouriteItem.FavouriteId,
                UserId = favouriteItem.UserId,  // Thêm thuộc tính UserId
                TicketId = favouriteItem.TicketId
                // Thêm các thuộc tính khác nếu cần
            }).ToList();

            return new ResponseDto()
            {
                Message = "Get favourites successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = favouritesDto
            };
        }

        public async Task<ResponseDto> UpdateFavourite(ClaimsPrincipal user, UpdateFavouriteDto updateFavouriteDto)
        {
            var favourite = await _unitOfWork.FavoriteRepository.GetById(updateFavouriteDto.FavouriteId);
            if (favourite == null)
            {
                return new ResponseDto
                {
                    Message = "Favourite not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            // Cập nhật thông tin favourite
            favourite.TicketId = updateFavouriteDto.TicketId;
            favourite.UpdatedBy = user.Identity.Name;
            favourite.UpdatedTime = DateTime.UtcNow;

            _unitOfWork.FavoriteRepository.Update(favourite);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Feedback updated successfully",
                Result = favourite,
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
