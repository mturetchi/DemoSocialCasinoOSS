namespace Bets.Contracts.Requests;

public class CreateBetRequest
{
    public Guid GameId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
    public Guid TransactionId { get; set; }
}
