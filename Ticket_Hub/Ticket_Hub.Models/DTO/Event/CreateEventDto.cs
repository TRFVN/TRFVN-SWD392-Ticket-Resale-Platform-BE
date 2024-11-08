namespace Ticket_Hub.Models.DTO.Event;

public class CreateEventDto
{
    public string EventName { get; set; } = null!;
    public string EventDescription { get; set; } = null!;
    public DateTime EventDate { get; set; }
    
    public string City { get; set; } = null!;
    public string District { get; set; } = null!;
    public string Address { get; set; } = null!;
}