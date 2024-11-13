using Wallets.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Wallets.Contracts.DTOs;

namespace Wallets.Application.Commands.Withdraw;

class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, WalletDTO>
{
    private readonly IWalletsWritableDbContext _dbContext;
    private readonly ILogger<WithdrawCommandHandler> _logger;

    public WithdrawCommandHandler(
        IWalletsWritableDbContext dbContext,
        ILogger<WithdrawCommandHandler> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<WalletDTO> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var fund = await _dbContext
            .Wallets
            .FirstOrDefaultAsync(f => f.CustomerId == request.CustomerId, cancellationToken);

        if (fund.Amount - request.Amount < 0)
            throw new Exception("Insufficient balance");

        fund.Amount -= request.Amount;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new WalletDTO
        {
            CustomerId = fund.CustomerId,
            Amount = fund.Amount,
        };
    }
}
