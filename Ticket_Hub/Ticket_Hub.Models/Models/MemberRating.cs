using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class MemberRating : BaseEntity<string, string, int>
    {
        [Key] public Guid MemberRatingId { get; set; }
        [StringLength(450)] public string UserId { get; set; } = null!;
        public int Rate { get; set; }

        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }
}