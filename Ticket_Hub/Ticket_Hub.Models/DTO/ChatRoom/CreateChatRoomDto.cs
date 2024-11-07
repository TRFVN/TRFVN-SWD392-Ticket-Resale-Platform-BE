namespace Ticket_Hub.Models.DTO.ChatRoom;

public class CreateChatRoomDto
{
    public Guid ChatRoomId { get; set; }
    public string NameRoom { get; set; } = null!;
    public Guid SendMessageUserId { get; set; }
    public Guid ReceiveMessageUserId { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}