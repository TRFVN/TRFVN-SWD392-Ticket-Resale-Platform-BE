using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Message
{
    public class CreateMessageDto
    {
        public string UserId { get; set; } = null!;
        public string MessageContent { get; set; } = null!;
    }
}
