﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Favourite
{
    public class CreateFavouriteDto
    {
        public string UserId { get; set; } = null!;
        public Guid TicketId { get; set; }
    }
}
