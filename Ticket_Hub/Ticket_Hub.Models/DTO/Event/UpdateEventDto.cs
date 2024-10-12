using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Models.DTO.Event;

public class UpdateEventDto : BaseEntity<string, string, int>
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = null!;
    public string EventDescription { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public Guid LocationId { get; set; }
}