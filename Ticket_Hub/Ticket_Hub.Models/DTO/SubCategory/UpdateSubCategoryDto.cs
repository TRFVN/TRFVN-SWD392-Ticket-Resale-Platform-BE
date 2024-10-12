using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.SubCategory;

public class UpdateSubCategoryDto
{
    public Guid SubCategoryId { get; set; }
    [StringLength(100)]
    public string SubCategoryName { get; set; } = null!;
    public Guid CategoryId { get; set; }
}