using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Cart;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public Task<ResponseDto> GetCart(ClaimsPrincipal User)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> AddToCart(ClaimsPrincipal User, AddToCartDTO addToCartDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return new ResponseDto()
                {
                    Message = "User was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var ticket = await _unitOfWork.TicketRepository.GetAsync(x => x.TicketId == addToCartDto.TicketId);
            if (ticket == null)
            {
                return new ResponseDto()
                {
                    Message = "Ticket was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            if (ticket.UserId == userId)
            {
                return new ResponseDto
                {
                    Message = "You cannot purchase your own ticket.",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }

            // Lấy giỏ hàng của người dùng  
            var cart = await _unitOfWork.CartRepository.GetAsync(x => x.UserId == userId);

            // Nếu giỏ hàng không tồn tại thì tạo giỏ hàng mới  
            if (cart == null)
            {
                cart = new Cart()
                {
                    CartId = Guid.NewGuid(),
                    UserId = userId,
                    TotalAmount = 0 // Khởi tạo total amount bằng 0  
                };
                await _unitOfWork.CartRepository.AddAsync(cart);
                await _unitOfWork.SaveAsync(); // Lưu giỏ hàng mới  
            }

            // Kiểm tra xem vé đã có trong giỏ hay chưa  
            var cartItem = await _unitOfWork.CartItemRepository.GetAsync(x =>
                x.TicketId == ticket.TicketId && x.CartId == cart.CartId);
            if (cartItem != null)
            {
                return new ResponseDto()
                {
                    IsSuccess = true,
                    Result = null,
                    StatusCode = 200,
                    Message = "A ticket already exists in the cart."
                };
            }

            // Cập nhật tổng số tiền trong giỏ hàng  
            cart.TotalAmount += ticket.TicketPrice;
            _unitOfWork.CartRepository.Update(cart); // Lưu giỏ hàng đã cập nhật  

            // Tạo và thêm item vào giỏ hàng  
            var newCartItem = new CartItem()
            {
                CartId = cart.CartId,
                TicketId = ticket.TicketId
            };

            await _unitOfWork.CartItemRepository.AddAsync(newCartItem);
            await _unitOfWork.SaveAsync(); // Lưu item mới thêm vào giỏ  

            // Chuyển đổi `cart` sang `CartDto` để tránh vòng lặp khi tuần tự hóa
            var cartDto = new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                TotalAmount = cart.TotalAmount,
                CartItems = cart.CartItems?.Select(item => new CartItemDto
                {
                    TicketId = item.TicketId,
                    TicketPrice = ticket.TicketPrice
                }).ToList()
            };

            return new ResponseDto
            {
                Message = "Ticket added to cart successfully.",
                IsSuccess = true,
                StatusCode = 200,
                Result = cartDto
            };
        }
        catch (Exception e)
        {
            return new ResponseDto()
            {
                Message = "An error occurred while adding the ticket to the cart: " + e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }

    public async Task<ResponseDto> RemoveFromCart(ClaimsPrincipal User, Guid TicketId)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return new ResponseDto()
            {
                Message = "User was not found",
                IsSuccess = false,
                StatusCode = 404,
                Result = null
            };
        }

        var cart = await _unitOfWork.CartRepository.GetAsync(x => x.UserId == userId);
        if (cart == null)
        {
            cart = new Cart()
            {
                CartId = Guid.NewGuid(),
                UserId = userId,
                TotalAmount = 0
            };
            await _unitOfWork.CartRepository.AddAsync(cart);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Message = "Cart was not found",
                IsSuccess = false,
                StatusCode = 404,
                Result = null
            };
        }

        var cartItems = await _unitOfWork.CartItemRepository.GetCartItemWithTicketAsync(cart.CartId, TicketId);
        if (cartItems == null)
        {
            return new ResponseDto()
            {
                Message = "Ticket was not found in the cart",
                IsSuccess = false,
                StatusCode = 404,
                Result = null
            };
        }

        cart.TotalAmount -= cartItems.Ticket.TicketPrice;
        _unitOfWork.CartRepository.Update(cart);

        _unitOfWork.CartItemRepository.Remove(cartItems);
        await _unitOfWork.SaveAsync();

        return new ResponseDto()
        {
            Message = "Ticket removed from cart successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = null
        };
    }

}