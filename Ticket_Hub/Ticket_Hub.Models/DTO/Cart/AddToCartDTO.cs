using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.Cart;

public class AddToCartDTO
{
    [Required] public Guid TicketId { get; set; }
}