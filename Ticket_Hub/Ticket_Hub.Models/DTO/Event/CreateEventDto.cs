namespace Ticket_Hub.Models.DTO.Event;

public class CreateEventDto
{
    public string EventName { get; set; } = null!;
    public string EventDescription { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public Guid LocationId { get; set; }
    
}