using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models;

public class Transactions
{
    [Key] public Guid TransactionId { get; set; }
    public Guid WalletId { get; set; }
    [ForeignKey("WalletId")] public virtual Wallet Wallet { get; set; } = null!;
    [StringLength(400)]public string Type { get; set; } = null!;
    public double Amount { get; set; }
    public DateTime TransactionDate  { get; set; }
}