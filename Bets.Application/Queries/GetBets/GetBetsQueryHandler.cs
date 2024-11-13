using Bets.Application.Interfaces;
using Bets.Contracts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bets.Application.Queries.GetBets;

class GetBetsQueryHandler : IRequestHandler<GetBetsQuery, List<BetDTO>>
{
    private readonly IBetsReadableDbContext _dbContext;

    public GetBetsQueryHandler(IBetsReadableDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<BetDTO>> Handle(GetBetsQuery request, CancellationToken cancellationToken) =>
        _dbContext
            .Bets
            .AsNoTracking()
            .Select(bet => new BetDTO
            {
                Id = bet.Id,
                Amount = bet.Amount,
                GameId = bet.GameId,
                TransactionId = bet.TransactionId,
                UserSessionId = bet.UserSessionId
            })
            .ToListAsync(cancellationToken);
}
