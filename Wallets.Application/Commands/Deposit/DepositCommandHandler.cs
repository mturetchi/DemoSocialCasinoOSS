using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wallets.Application.Interfaces;
using Wallets.Contracts.DTOs;
using Wallets.Core.Entities;

namespace Wallets.Application.Commands.Deposit;

class DepositCommandHandler : IRequestHandler<DepositCommand, WalletDTO>
{
    private readonly IWalletsWritableDbContext _dbContext;
    private readonly ILogger<DepositCommandHandler> _logger;

    public DepositCommandHandler(
        IWalletsWritableDbContext dbContext,
        ILogger<DepositCommandHandler> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<WalletDTO> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _dbContext
            .Wallets
            .FirstOrDefaultAsync(f => f.CustomerId == request.CustomerId, cancellationToken);

        if (wallet == null)
        {
            wallet = new Wallet
            {
                Amount = request.Amount,
                CustomerId = request.CustomerId
            };
            _dbContext.Wallets.Add(wallet);
        }

        wallet.Amount += request.Amount;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new WalletDTO
        {
            CustomerId = wallet.CustomerId,
            Amount = wallet.Amount,
        };
    }
}
