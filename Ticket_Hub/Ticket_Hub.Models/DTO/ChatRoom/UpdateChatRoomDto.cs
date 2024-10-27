namespace Ticket_Hub.Models.DTO.ChatRoom;

public class UpdateChatRoomDto
{
    public Guid ChatRoomId { get; set; }
    public string NameRoom { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}