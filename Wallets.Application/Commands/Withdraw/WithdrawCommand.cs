using MediatR;
using Wallets.Contracts.DTOs;
using Wallets.Contracts.Requests;

namespace Wallets.Application.Commands.Withdraw;

public class WithdrawCommand : WithdrawRequest, IRequest<WalletDTO>
{
}
