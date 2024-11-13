using Bets.Contracts.DTOs;
using Bets.Contracts.Requests;
using MediatR;

namespace Bets.Application.Commands.CreateBets;

public class CreateBetCommand : CreateBetRequest, IRequest<BetDTO>
{

}
