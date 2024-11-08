using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository;

public interface INegotiationsRepository : IRepository<Negotiations>
{
    Task<IEnumerable<Message>> GetMessagesByChatRoomIdAsync(Guid chatRoomId);
    Task<IEnumerable<Message>> GetMessagesByUserIdAsync(Guid userId);
    void Update(Negotiations negotiations);
}