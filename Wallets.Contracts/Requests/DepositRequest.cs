namespace Wallets.Contracts.Requests;

public class DepositRequest
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}
