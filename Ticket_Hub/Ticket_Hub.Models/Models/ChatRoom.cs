using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;

public class ChatRoom 
{
    [Key]
    public Guid ChatRoomId { get; set; }
    [StringLength(20)] 
    public string NameRoom { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}