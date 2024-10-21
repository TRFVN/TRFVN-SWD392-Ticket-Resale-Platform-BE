using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.CartDetail
{
    public class UpdateCartDetailDto
    {
        public Guid CartDetailId { get; set; }
        public Guid TicketId { get; set; }
        public double TicketPrice { get; set; }
    }
}
