using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.MemberRating
{
    public class CreateMemberRatingDto
    {
        public Guid UserId { get; set; }
        public int Rate { get; set; } = 0;
    }
}
