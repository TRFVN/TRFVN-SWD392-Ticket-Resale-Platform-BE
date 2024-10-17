using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Feedback
{
    public class UpdateFeedbackDto
    {
        public Guid FeedbackId { get; set; }
        public string? Content { get; set; }
    }
}
