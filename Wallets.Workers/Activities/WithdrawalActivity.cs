using MediatR;
using Temporalio.Activities;
using Temporalio.Exceptions;
using Wallets.Application.Commands.Withdraw;
using Wallets.Contracts.DTOs;
using Wallets.Contracts.Requests;

namespace Wallets.Workers.Activities;

public class WithdrawalActivity
{
    private readonly IMediator _mediator;

    public WithdrawalActivity(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Activity]
    public Task<WalletDTO> WithdrawAsync(WithdrawRequest request)
    {
        try
        {
            return _mediator.Send(new WithdrawCommand
            {
                Amount = request.Amount,
                CustomerId = request.CustomerId
            });
        }
        catch (Exception ex)
        {
            throw new ApplicationFailureException("Failed to withdraw", ex);
        }
    }
}
