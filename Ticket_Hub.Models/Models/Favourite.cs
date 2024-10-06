using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class Favourite
    {
        [Key] public Guid FavouriteId { get; set; }
        [StringLength(450)] public string UserId { get; set; } = null!;
        public Guid TicketId { get; set; }

        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }
}