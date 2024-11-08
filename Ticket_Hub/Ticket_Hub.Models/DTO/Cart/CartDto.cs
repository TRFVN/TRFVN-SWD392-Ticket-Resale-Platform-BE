namespace Ticket_Hub.Models.DTO.Cart;

public class CartDto

{
    public Guid CartId { get; set; }
    public string UserId { get; set; }
    public double TotalAmount { get; set; }
    public List<CartItemDto> CartItems { get; set; }
}