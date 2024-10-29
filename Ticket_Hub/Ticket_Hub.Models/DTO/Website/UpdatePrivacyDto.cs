namespace Ticket_Hub.Models.DTO.Website;

public class UpdatePrivacyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
}
