using MediatR;

namespace Wallets.Application.Queries.CheckWallets;

public class CheckWalletsQuery : IRequest<bool>
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
