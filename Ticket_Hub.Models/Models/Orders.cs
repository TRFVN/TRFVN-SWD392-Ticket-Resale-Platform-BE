using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Models
{
    public class Orders : BaseEntity<string, string, int>
    {
        [Key] public Guid OrderId { get; set; }
        public Guid CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")] public virtual CartHeader CartHeader { get; set; } = null!;
        public Guid TicketId { get; set; }
        public double TotalPrice { get; set; }
    }
}