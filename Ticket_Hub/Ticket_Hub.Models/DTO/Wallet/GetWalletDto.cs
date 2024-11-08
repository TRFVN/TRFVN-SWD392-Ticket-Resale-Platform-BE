namespace Ticket_Hub.Models.DTO.Wallet;

public class GetWalletDto
{
    public Guid WalletId { get; set; }
    public double TotalBalance { get; set; }
    public double PayoutBalance { get; set; }
    public DateTime UpdateTime { get; set; }
    public string UserId { get; set; } = null!;
}