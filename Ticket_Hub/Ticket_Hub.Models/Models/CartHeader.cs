using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class CartHeader : BaseEntity<string, string, int>
    {
        [Key] public Guid CartHeaderId { get; set; }
        [StringLength(450)] public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public int AmountTicket { get; set; }
        public double TotalPrice { get; set; }

        public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}