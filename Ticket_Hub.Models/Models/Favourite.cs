using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class Favourite
    {
        [Key] public Guid FavouriteId { get; set; }
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }

        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }
}