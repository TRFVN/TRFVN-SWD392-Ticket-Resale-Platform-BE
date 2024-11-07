using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Models
{
    public class Orders
    {
        [Key] public Guid OrderId { get; set; }
        [StringLength(450)] public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public double TotalPrice { get; set; }
        public virtual ICollection<OrderTicket> OrderTickets { get; set; } = new List<OrderTicket>();
    }
}