using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;


public class Negotiations
{
    [Key]public Guid NegotiationId { get; set; }
    public Guid ChatRoomId { get; set; }
    [ForeignKey("ChatRoomId")] public virtual ChatRoom ChatRoom { get; set; } = null!;
    public Guid MessageId { get; set; }
    [ForeignKey("MessageId")] public virtual Message Message { get; set; } = null!;
    public Guid TicketId { get; set; }
    [ForeignKey("TicketId")] public virtual Ticket Ticket { get; set; } = null!;
    public double Price { get; set; }
    public bool Status { get; set; }
}