using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;

public enum NegotiationStatus  
{  
    Pending,  
    Success,  
    Cancel  
}  

public class Negotiations
{
    [Key]public Guid NegotiationId { get; set; }
    public Guid ChatRoomId { get; set; }
    [ForeignKey("ChatRoomId")] public virtual ChatRoom ChatRoom { get; set; } = null!;
    public Guid MessageId { get; set; }
    public double Price { get; set; }
    public NegotiationStatus Status { get; set; }
}