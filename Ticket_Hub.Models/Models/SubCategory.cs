using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;

public class SubCategory
{
    [Key]
    public Guid SubCategoryId { get; set; }
    [StringLength(100)]
    public string SubCategoryName { get; set; } = null!;
    public Guid CategoryId { get; set; }
    [ForeignKey("CategoryId")] public Category Category { get; set; } = null!;
}