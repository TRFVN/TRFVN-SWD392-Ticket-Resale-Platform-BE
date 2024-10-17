using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Favourite
{
    public class UpdateFavouriteDto
    {
        public Guid FavouriteId { get; set; }
        public Guid TicketId { get; set; }
    }
}
