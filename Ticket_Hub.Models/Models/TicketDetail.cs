using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class TicketDetail
    {
        [Key] public Guid TicketDetailId { get; set; }
        public Guid TicketId { get; set; }
        [ForeignKey("TicketId")] public virtual Ticket Ticket { get; set; } = null!;
        public DateTime PurchaseDate { get; set; }
        [StringLength(500)] 
        public string TicketImageUrl { get; set; } = null!;
    }
}