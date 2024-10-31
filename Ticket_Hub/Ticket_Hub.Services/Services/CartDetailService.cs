using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.CartDetail;
using Ticket_Hub.Models.DTO.CartHeader;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services
{
    public class CartDetailService : ICartDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CartDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetCartDetails(ClaimsPrincipal user, string? filterOn, string? filterQuery, string? sortBy, int pageNumber = 0, int pageSize = 0)
        {
            IEnumerable<CartDetail> allCartDetails = null!;
            allCartDetails = await _unitOfWork.CartDetailRepository.GetAllAsync();
            if (!allCartDetails.Any())
            {
                return new ResponseDto
                {
                    Message = "There are no cart details",
                    IsSuccess = true,
                    StatusCode = 404,
                    Result = null
                };
            }

            var listCartDetails = allCartDetails.ToList();
            if (!listCartDetails.Any())
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "ticketprice":
                        if (double.TryParse(filterQuery, out double price))
                        {
                            listCartDetails = listCartDetails
                            .Where(x => x.TicketPrice == price).ToList();
                        }
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "ticketprice":
                        listCartDetails = listCartDetails.OrderBy(x => x.TicketPrice).ToList();
                        break;
                    default:
                        // Nếu không có giá trị `sortBy` hợp lệ, sắp xếp theo `TicketPrice` giảm dần
                        listCartDetails = listCartDetails.OrderByDescending(x => x.TicketPrice).ToList();
                        break;
                }
            }
            else
            {
                // Sắp xếp mặc định nếu không có `sortBy`
                listCartDetails = listCartDetails.OrderByDescending(x => x.TicketPrice).ToList();
            }

            // Phân trang
            if (pageNumber > 0 && pageSize > 0)
            {
                var skipResult = (pageNumber - 1) * pageSize;
                listCartDetails = listCartDetails.Skip(skipResult).Take(pageSize).ToList();
            }

            // Chuyển đổi danh sách thành DTO
            var cartDetailDto = listCartDetails.Select(item => new GetCartDetailDto()
            {
                CartDetailId = item.CartDetailId,
                CartHeaderId = item.CartHeaderId,
                TicketId = item.TicketId,
                TicketPrice = item.TicketPrice,
            }).ToList();

            return new ResponseDto()
            {
                Message = "Get cart details successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = cartDetailDto,
            };
        }

        public async Task<ResponseDto> GetCartDetail(ClaimsPrincipal user, Guid cartDetailId)
        {
            var cartDetail = await _unitOfWork.CartDetailRepository.GetById(cartDetailId);
            if (cartDetail == null)
            {
                return new ResponseDto
                {
                    Message = "Cart detail not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var cartDetailDto = _mapper.Map<GetCartDetailDto>(cartDetail);

            return new ResponseDto
            {
                Message = "Cart detail found successfully",
                Result = cartDetailDto,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> CreateCartDetail(ClaimsPrincipal user, CreateCartDetailDto createCartDetailDto)
        {
            CartDetail newCartDetail = new CartDetail()
            {
                CartHeaderId = createCartDetailDto.CartHeaderId,
                TicketId = createCartDetailDto.TicketId,
                TicketPrice = createCartDetailDto.TicketPrice,
                CreatedBy = user.Identity.Name,
                CreatedTime = DateTime.Now,
                UpdatedBy = "",
                UpdatedTime = null,
                Status = 1,
            };

            await _unitOfWork.CartDetailRepository.AddAsync(newCartDetail);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Cart detail created successfully",
                Result = newCartDetail,
                IsSuccess = true,
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> DeleteCartDetail(ClaimsPrincipal user, Guid cartDetailId)
        {
            var cartDetail = await _unitOfWork.CartDetailRepository.GetAsync(x => x.CartDetailId == cartDetailId);
            if (cartDetail == null)
            {
                return new ResponseDto
                {
                    Message = "Cart detail not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            cartDetail.Status = 0;
            cartDetail.UpdatedBy = user.Identity.Name;
            cartDetail.UpdatedTime = DateTime.UtcNow;

            //save changes
            _unitOfWork.CartDetailRepository.Update(cartDetail);
            var save = await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Message = "Cart header delete successfully",
                Result = cartDetail,
                IsSuccess = true,
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> UpdateCartDetail(ClaimsPrincipal user, UpdateCartDetailDto updateCartDetailDto)
        {
            var cartDetailId =
            await _unitOfWork.CartDetailRepository.GetAsync(x => x.CartDetailId == updateCartDetailDto.CartDetailId);
            if (cartDetailId == null)
            {
                return new ResponseDto
                {
                    Message = "Cart detail not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            //update CartDetail
            cartDetailId.TicketId = updateCartDetailDto.TicketId;
            cartDetailId.TicketPrice = updateCartDetailDto.TicketPrice;
            cartDetailId.UpdatedBy = user.Identity.Name;
            cartDetailId.UpdatedTime = DateTime.Now;

            //save changes
            _unitOfWork.CartDetailRepository.Update(cartDetailId);
            var save = await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Cart detail updated successfully",
                Result = cartDetailId,
                IsSuccess = true,
                StatusCode = 201
            };
        }
    }
}
