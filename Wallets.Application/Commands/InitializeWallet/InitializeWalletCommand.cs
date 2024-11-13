using MediatR;
using Wallets.Contracts.Requests;

namespace Wallets.Application.Commands.InitializeWallet;

public class InitializeWalletCommand : InitializeWalletRequest, IRequest
{

}
