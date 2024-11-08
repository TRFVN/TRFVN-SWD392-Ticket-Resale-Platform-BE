namespace Ticket_Hub.Models.DTO.Transaction;

public class CreateTransactionDto
{
    public Guid TransactionId { get; set; }
    public Guid WalletId { get; set; }
    public string Type { get; set; } = null!;
    public double Amount { get; set; }
    public DateTime TransactionDate  { get; set; }
}