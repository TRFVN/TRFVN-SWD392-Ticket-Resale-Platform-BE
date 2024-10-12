using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.Category;

public class GetCategoryDto
{
    public Guid CategoryId { get; set; }
    [StringLength(50)] public string CategoryName { get; set; } = null!;
}