namespace Ticket_Hub.Models.DTO.Negotiation;

public class CreateNegotiationsDto
{
    public Guid ChatRoomId { get; set; }
    public Guid MessageId { get; set; }
    public Guid TicketId { get; set; }
    public double Price { get; set; }
}