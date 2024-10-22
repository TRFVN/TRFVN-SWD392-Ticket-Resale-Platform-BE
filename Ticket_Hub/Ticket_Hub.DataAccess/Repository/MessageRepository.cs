using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Message> GetById(Guid messageId)
        {
            return await _context.Messages.FirstOrDefaultAsync(x => x.MessageId == messageId);
        }

        public void Update(Message message)
        {
            _context.Messages.Update(message);
        }

        public void UpdateRange(IEnumerable<Message> messages)
        {
            _context.Messages.UpdateRange(messages);
        }
    }
}
