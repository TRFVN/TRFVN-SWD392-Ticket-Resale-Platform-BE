namespace Ticket_Hub.Models.DTO.Category;

public class UpdateCategoryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
}