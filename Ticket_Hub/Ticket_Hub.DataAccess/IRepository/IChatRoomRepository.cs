using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IChatRoomRepository : IRepository<ChatRoom>
{
    void Update(ChatRoom chatRoom);
    void UpdateRange(IEnumerable<ChatRoom> chatRooms);
    Task<ChatRoom> GetById(Guid chatRoomId);
}