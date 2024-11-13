using Bets.Application.Interfaces;
using Bets.Contracts.DTOs;
using Bets.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bets.Application.Commands.CreateBets;

class CreateBetCommandHandler : IRequestHandler<CreateBetCommand, BetDTO>
{
    private readonly IBetsWritableDbContext _dbContext;
    private readonly ILogger<CreateBetCommandHandler> _logger;

    public CreateBetCommandHandler(
        IBetsWritableDbContext dbContext,
        ILogger<CreateBetCommandHandler> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<BetDTO> Handle(CreateBetCommand request, CancellationToken cancellationToken)
    {
        var bet = new Bet
        {
            Amount = request.Amount,
            GameId = request.GameId,
            TransactionId = request.TransactionId,
            UserSessionId = request.UserId,
        };

        _dbContext.Bets.Add(bet);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new BetDTO
        {
            Id = bet.Id,
            Amount = bet.Amount,
            GameId = bet.GameId,
            TransactionId = bet.TransactionId,
            UserSessionId = bet.UserSessionId
        };
    }
}
