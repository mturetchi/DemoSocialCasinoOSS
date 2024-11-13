using Core.Shared;

namespace Wallets.Core.Entities;

public class Wallet : BaseEntity
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}
