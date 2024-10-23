using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.CartHeader
{
    public class UpdateCartHeaderDto
    {
        public Guid CartHeaderId { get; set; }
        public int AmountTicket { get; set; }
        public double TotalPrice { get; set; }
    }
}
