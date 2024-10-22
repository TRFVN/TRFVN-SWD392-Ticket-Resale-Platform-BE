using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.CartDetail
{
    public class GetCartDetailDto
    {
        public Guid CartDetailId { get; set; }
        public Guid CartHeaderId { get; set; }
        public Guid TicketId { get; set; }
        public double TicketPrice { get; set; }
    }
}
