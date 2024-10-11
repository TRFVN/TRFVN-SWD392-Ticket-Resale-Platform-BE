using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Favourite
{
    public class FavouriteDto
    {
        public Guid FavouriteId { get; set; }
        public string UserId { get; set; } = null!;
        public Guid TicketId { get; set; }
    }
}
