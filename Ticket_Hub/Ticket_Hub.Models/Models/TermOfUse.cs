namespace Ticket_Hub.Models.Models;

public class TermOfUse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsActive { get; set; }
}