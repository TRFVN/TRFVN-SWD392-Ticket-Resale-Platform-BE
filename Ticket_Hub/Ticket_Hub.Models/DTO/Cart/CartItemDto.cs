namespace Ticket_Hub.Models.DTO.Cart;

public class CartItemDto
{
    public Guid CartId { get; set; }  
    public Guid TicketId { get; set; }  
    public double TicketPrice { get; set; }
}