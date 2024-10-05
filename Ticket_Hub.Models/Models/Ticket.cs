using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class Ticket
    {
        [Key] public Guid TicketId { get; set; }
        [StringLength(500)] 
        public string TicketName { get; set; } = null!;
        public Guid EventId { get; set; }
        [ForeignKey("EventId")] public virtual Event Event { get; set; } = null!;
        public Guid UserId { get; set; }
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")] public virtual Category Category { get; set; } = null!;
        public double TicketPrice { get; set; }
        public int TicketQuantity { get; set; }
        [StringLength(500)] 
        public string TicketDescription { get; set; } = null!;
        [StringLength(20)] 
        public string SerialNumber { get; set; } = null!;
        public int Status { get; set; }
        
        public virtual ICollection<TicketDetail>? TicketDetails { get; set; }
        
    }
}