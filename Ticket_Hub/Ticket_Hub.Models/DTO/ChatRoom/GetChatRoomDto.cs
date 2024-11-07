using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.ChatRoom;

public class GetChatRoomDto
{
    public Guid ChatRoomId { get; set; }
    [StringLength(20)] 
    public string NameRoom { get; set; } = null!;
    public string SendMessageUserId { get; set; }
    public string ReceiveMessageUserId { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}