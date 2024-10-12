using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.SubCategory;

public class CreateSubCategoryDto
{
    [StringLength(100)]
    public string SubCategoryName { get; set; } = null!;
    public Guid CategoryId { get; set; }
}