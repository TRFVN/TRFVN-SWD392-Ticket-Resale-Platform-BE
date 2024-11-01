﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.IRepository
{
    public interface IMessageRepository : IRepository<Message>
    {
        void Update(Message message);
        void UpdateRange(IEnumerable<Message> messages);
        Task<Message> GetById(Guid messageId);
    }
}
