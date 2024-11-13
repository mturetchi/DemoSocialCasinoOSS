using Wallets.Application.Interfaces;
using MediatR;
using Wallets.Core.Entities;

namespace Wallets.Application.Commands.InitializeWallet;

class InitializeWalletsCommandHandler : IRequestHandler<InitializeWalletCommand>
{
    private readonly IWalletsWritableDbContext _dbContext;

    public InitializeWalletsCommandHandler(IWalletsWritableDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(InitializeWalletCommand request, CancellationToken cancellationToken)
    {
        _dbContext.Wallets.Add(new Wallet
        {
            Amount = request.Amount,
            CustomerId = request.UserId
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
