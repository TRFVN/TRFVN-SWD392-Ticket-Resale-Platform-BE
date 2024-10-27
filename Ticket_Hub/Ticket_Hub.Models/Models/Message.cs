using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;

public class Message
{
    [Key]
    public Guid MessageId { get; set; }
    [StringLength(200)] 
    public string MessageContent { get; set; } = null!;
    [StringLength(450)] public string UserId { get; set; } = null!;
    [ForeignKey("UserId")] public ApplicationUser User { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public Guid? ChatRoomId { get; set; }
    [ForeignKey("ChatRoomId")] 
    public virtual ChatRoom ChatRoom { get; set; }
}