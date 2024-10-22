using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Feedback
{
    public class GetFeedbackDto
    {
        public Guid FeedbackId { get; set; }
        public required string UserId { get; set; } = null!;
        public string? Content { get; set; }
    }
}
