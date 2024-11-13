using Core.Shared;

namespace Bets.Core.Entities;

public class Bet : BaseEntity
{
    public Guid GameId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserSessionId { get; set; }
    public Guid TransactionId { get; set; }
}
