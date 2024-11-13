namespace Bets.Contracts.DTOs;

public class BetDTO
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserSessionId { get; set; }
    public Guid TransactionId { get; set; }
    public decimal WalletAmount { get; set; }
}
