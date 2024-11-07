using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;

public class Category : BaseEntity<string, string, int>
{
    [Key] public Guid CategoryId { get; set; }
    [StringLength(50)] public string CategoryName { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }

    [ForeignKey("ParentCategoryId")]
    public virtual Category? ParentCategory { get; set; }
}