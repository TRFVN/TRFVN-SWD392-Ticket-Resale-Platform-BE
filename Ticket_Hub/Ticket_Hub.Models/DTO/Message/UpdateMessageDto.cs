using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Message
{
    public class UpdateMessageDto
    {
        public Guid MessageId { get; set; }
        public string MessageContent { get; set; } = null!;
    }
}
