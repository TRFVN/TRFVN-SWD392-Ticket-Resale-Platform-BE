using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Message
{
    public class CreateMessageDto
    {
        public Guid MessageId { get; set; }
        public string MessageContent { get; set; } = null!;
        public Guid SendMessageUserId { get; set; }  
        public Guid ReceiveMessageUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid? ChatRoomId { get; set; }
    }
}
