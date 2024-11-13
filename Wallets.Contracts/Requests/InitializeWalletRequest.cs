namespace Wallets.Contracts.Requests;

public class InitializeWalletRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
