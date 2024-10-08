using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class Feedback : BaseEntity<string, string, int>
    {
        [Key] public Guid FeedbackId { get; set; }
        [StringLength(450)] public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        [StringLength(500)] public string? Content { get; set; }
    }
}