using System.ComponentModel.DataAnnotations;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Models.DTO.SubCategory;

public class GetSubCategoryDto : BaseEntity<string, string, int>
{
    public Guid SubCategoryId { get; set; }
    [StringLength(100)]
    public string SubCategoryName { get; set; } = null!;
    public Guid CategoryId { get; set; }
}