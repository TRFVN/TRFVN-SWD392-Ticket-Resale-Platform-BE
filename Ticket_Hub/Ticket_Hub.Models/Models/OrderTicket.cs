using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;

public class OrderTicket
{
    public Guid OrderId { get; set; }
    [ForeignKey("OrderId")] public virtual Orders Orders { get; set; } = null!;
    public Guid TicketId { get; set; }
    [ForeignKey("TicketId")] public virtual Ticket Ticket { get; set; } = null!;
}