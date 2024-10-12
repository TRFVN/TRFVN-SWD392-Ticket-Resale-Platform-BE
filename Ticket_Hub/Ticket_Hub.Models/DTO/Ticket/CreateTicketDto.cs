namespace Ticket_Hub.Models.DTO.Ticket;

public class CreateTicketDto
{
    public string TicketName { get; set; } = null!;
    public string TicketDescription { get; set; } = null!;
    public Guid EventId { get; set; }
    public Guid CategoryId { get; set; }
    public double TicketPrice { get; set; }
    public int TicketQuantity { get; set; }
    public string SerialNumber { get; set; } = null!;
    public int Status { get; set; }
}