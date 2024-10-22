using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.CartDetail
{
    public class CreateCartDetailDto
    {
        public Guid CartHeaderId { get; set; }
        public Guid TicketId { get; set; }
        public double TicketPrice { get; set; }
    }
}
