using Wallets.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Wallets.Application.Queries.CheckWallets;

class CheckWalletsQueryHandler : IRequestHandler<CheckWalletsQuery, bool>
{
    private readonly IWalletsReadableDbContext _dbContext;
    private readonly ILogger<CheckWalletsQueryHandler> _logger;

    public CheckWalletsQueryHandler(
        IWalletsReadableDbContext dbContext,
        ILogger<CheckWalletsQueryHandler> logger    
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> Handle(CheckWalletsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var stopWatch = Stopwatch.StartNew();
            var fund = await _dbContext
                .Wallets
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.CustomerId == request.UserId);
            stopWatch.Stop();
            _logger.LogInformation($"SQL (CheckWallets): {stopWatch.ElapsedMilliseconds}");

            if (fund == null)
                return false;

            return fund.Amount - request.Amount >= 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
