using MediatR;
using Wallets.Contracts.DTOs;

namespace Wallets.Application.Queries.GetWallets;

public class GetWalletsQuery : IRequest<WalletDTO>
{
    public Guid CustomerId { get; set; }
}
