using MediatR;
using Wallets.Contracts.DTOs;
using Wallets.Contracts.Requests;
namespace Wallets.Application.Commands.Deposit;

public class DepositCommand : DepositRequest, IRequest<WalletDTO>
{
}
