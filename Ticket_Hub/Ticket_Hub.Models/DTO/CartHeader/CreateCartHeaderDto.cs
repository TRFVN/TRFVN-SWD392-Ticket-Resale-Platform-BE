using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.CartHeader
{
    public class CreateCartHeaderDto
    {
        public string UserId { get; set; } = null!;
        public int AmountTicket { get; set; }
        public double TotalPrice { get; set; }
    }
}
