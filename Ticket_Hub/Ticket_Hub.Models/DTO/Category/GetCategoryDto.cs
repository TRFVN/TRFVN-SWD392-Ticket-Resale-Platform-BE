using System.ComponentModel.DataAnnotations;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Models.DTO.Category;

public class GetCategoryDto : BaseEntity<string, string, int>
{
    public Guid CategoryId { get; set; }
    [StringLength(50)] public string CategoryName { get; set; } = null!;
}