using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.Models;

public class Category : BaseEntity<string, string, int>
{
    [Key] public Guid CategoryId { get; set; }
    [StringLength(50)] public string CategoryName { get; set; } = null!;
    public virtual ICollection<SubCategory>? SubCategories { get; set; }
}