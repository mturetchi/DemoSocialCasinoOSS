using Wallets.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallets.Contracts.DTOs;

namespace Wallets.Application.Queries.GetWallets;

class GetWalletsQueryHandler : IRequestHandler<GetWalletsQuery, WalletDTO>
{
    private readonly IWalletsReadableDbContext _dbContext;

    public GetWalletsQueryHandler(IWalletsReadableDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<WalletDTO> Handle(GetWalletsQuery request, CancellationToken cancellationToken) =>
        _dbContext
            .Wallets
            .AsNoTracking()
            .Select(f => new WalletDTO
            {
                CustomerId = f.CustomerId,
                Amount = f.Amount,
            }).FirstOrDefaultAsync(f => f.CustomerId == request.CustomerId, cancellationToken);
}
