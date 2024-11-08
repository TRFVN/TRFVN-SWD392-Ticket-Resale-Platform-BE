using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository;

public class NegotiationsRepository : Repository<Negotiations>, INegotiationsRepository
{
    private readonly ApplicationDbContext _context;
    
    public NegotiationsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Message>> GetMessagesByChatRoomIdAsync(Guid chatRoomId)
    {
        return await _context.Messages
            .Where(m => m.ChatRoomId == chatRoomId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetMessagesByUserIdAsync(Guid userId)
    {
        return await _context.Messages
            .Where(m => m.SendMessageUserId == userId || m.ReceiveMessageUserId == userId)
            .ToListAsync();
    }

    public void Update(Negotiations negotiations)
    {
        _context.Negotiations.Update(negotiations);
    }
}