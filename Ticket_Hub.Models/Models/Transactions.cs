using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;

public class Transactions
{
    [Key] public Guid TransactionId { get; set; }
    public Guid WalletId { get; set; }
    [ForeignKey("WalletId")] public virtual Wallet Wallet { get; set; } = null!;
    public int Type { get; set; }
    public double Amount { get; set; }
    public DateTime CreateTime { get; set; }
}