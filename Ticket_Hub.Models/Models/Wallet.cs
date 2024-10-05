using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class Wallet
    {
        [Key]
        public Guid WalletId { get; set; }
        public double TotalBalance { get; set; }
        public double AvailableBalance { get; set; }
        public double WithdrawnBalance { get; set; }
        public string Currencies { get; set; } = null!;
        public DateTime UpdateTime { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")] public virtual ApplicationUser User { get; set; } = null!;
    }
}
