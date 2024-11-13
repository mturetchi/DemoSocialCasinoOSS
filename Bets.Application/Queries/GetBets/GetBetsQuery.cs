using Bets.Contracts.DTOs;
using MediatR;

namespace Bets.Application.Queries.GetBets;

public class GetBetsQuery : IRequest<List<BetDTO>>
{
}
