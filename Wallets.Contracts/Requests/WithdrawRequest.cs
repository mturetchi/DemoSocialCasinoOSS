namespace Wallets.Contracts.Requests;

public class WithdrawRequest
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}
