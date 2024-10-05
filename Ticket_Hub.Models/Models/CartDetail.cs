using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class CartDetail : BaseEntity<string, string, int>
    {
        [Key]
        public Guid CartDetailId { get; set; }
        public Guid CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")] public virtual CartHeader CartHeader { get; set; } = null!;
        public Guid TicketId { get; set; }
        public double TicketPrice { get; set; }

    }
}
