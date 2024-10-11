using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.MemberRating
{
    public class MemberRatingDto
    {
        public Guid MemberRatingId { get; set; }
        public string UserId { get; set; } = null!;
        public int Rate { get; set; }
    }
}
