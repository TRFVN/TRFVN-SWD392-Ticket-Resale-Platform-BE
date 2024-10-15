using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.MemberRating
{
    public class GetMemberRatingDto
    {
        public Guid MemberRatingId { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
    }
}
