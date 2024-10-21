using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.CartHeader;
using Ticket_Hub.Models.DTO.Category;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services
{
    public class CartHeaderService : ICartHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CartHeaderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetCartHeaders(ClaimsPrincipal user, string? filterOn, string? filterQuery, string? sortBy, int pageNumber = 0, int pageSize = 0)
        {
            IEnumerable<CartHeader> allCartHeaders = null!;
            allCartHeaders = await _unitOfWork.CartHeaderRepository.GetAllAsync();
            if (!allCartHeaders.Any())
            {
                return new ResponseDto
                {
                    Message = "There are no cart headers",
                    IsSuccess = true,
                    StatusCode = 404,
                    Result = null
                };
            }

            var listCartHeaders = allCartHeaders.ToList();

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "userid":
                        listCartHeaders = listCartHeaders
                            .Where(x => x.UserId.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase))
                            .ToList();
                        break;
                    default:
                        break;
                }
            }

            var cartHeaderDto = listCartHeaders.Select(item => new GetCartHeaderDto()
            {
                CartHeaderId = item.CartHeaderId,
                UserId = item.UserId,
                AmountTicket = item.AmountTicket,
                TotalPrice = item.TotalPrice
            }).ToList();

            return new ResponseDto()
            {
                Message = "Get cart headers successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = cartHeaderDto
            };
        }

        public async Task<ResponseDto> GetCartHeader(ClaimsPrincipal user, Guid cartHeaderId)
        {
            var cartHeader = await _unitOfWork.CartHeaderRepository.GetById(cartHeaderId);
            if (cartHeader == null)
            {
                return new ResponseDto
                {
                    Message = "Cart header not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var cartHeaderDto = _mapper.Map<GetCartHeaderDto>(cartHeader);

            return new ResponseDto
            {
                Message = "Cart header found successfully",
                Result = cartHeaderDto,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> CreateCartHeader(ClaimsPrincipal user, CreateCartHeaderDto createCartHeaderDto)
        {
            CartHeader newCartHeader = new CartHeader()
            {
                UserId = createCartHeaderDto.UserId,
                AmountTicket = createCartHeaderDto.AmountTicket,
                TotalPrice = createCartHeaderDto.TotalPrice,
                CreatedBy = user.Identity.Name,
                CreatedTime = DateTime.UtcNow,
                UpdatedBy = "",
                UpdatedTime = null,
                Status = 1,
            };

            await _unitOfWork.CartHeaderRepository.AddAsync(newCartHeader);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Cart header created successfully",
                Result = newCartHeader,
                IsSuccess = true,
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> UpdateCartHeader(ClaimsPrincipal user, UpdateCartHeaderDto updateCartHeaderDto)
        {
            var cartHeaderId =
            await _unitOfWork.CartHeaderRepository.GetAsync(x => x.CartHeaderId == updateCartHeaderDto.CartHeaderId);
            if (cartHeaderId == null)
            {
                return new ResponseDto
                {
                    Message = "Cart header not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            //update CartHeader
            var cartHeader_AllDetails = _unitOfWork.CartDetailRepository.GetAll(cartHeaderId.CartHeaderId);
            if (cartHeader_AllDetails == null)
            {
                cartHeaderId.AmountTicket = 0;
                cartHeaderId.TotalPrice = 0;
            } else
            {
                var listTicketInCart = cartHeader_AllDetails.ToList();
                cartHeaderId.AmountTicket = listTicketInCart.Count;
                cartHeaderId.TotalPrice = listTicketInCart.Sum(x => x.TicketPrice);
            }
            cartHeaderId.UpdatedBy = user.Identity.Name;
            cartHeaderId.UpdatedTime = DateTime.Now;

            //save changes
            _unitOfWork.CartHeaderRepository.Update(cartHeaderId);
            var save = await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Cart header updated successfully",
                Result = cartHeaderId,
                IsSuccess = true,
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> DeleteCartHeader(ClaimsPrincipal user, Guid cartHeaderId)
        {
            var cartHeader = await _unitOfWork.CartHeaderRepository.GetAsync(x => x.CartHeaderId == cartHeaderId);
            if (cartHeader == null)
            {
                return new ResponseDto
                {
                    Message = "Cart header not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            cartHeader.Status = 0;
            cartHeader.UpdatedBy = user.Identity.Name;
            cartHeader.UpdatedTime = DateTime.UtcNow;

            //save changes
            _unitOfWork.CartHeaderRepository.Update(cartHeader);
            var save = await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Message = "Cart header delete successfully",
                Result = cartHeader,
                IsSuccess = true,
                StatusCode = 201
            };

        }
    }
}
