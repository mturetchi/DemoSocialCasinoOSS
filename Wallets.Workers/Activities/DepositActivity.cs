using MediatR;
using Temporalio.Activities;
using Temporalio.Exceptions;
using Wallets.Application.Commands.Deposit;
using Wallets.Contracts.DTOs;
using Wallets.Contracts.Requests;

namespace Wallets.Workers.Activities;

public class DepositActivity
{
    private readonly IMediator _mediator;

    public DepositActivity(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Activity]
    public Task<WalletDTO> DepositAsync(DepositRequest request)
    {
        try
        {
            return _mediator.Send(new DepositCommand
            {
                Amount = request.Amount,
                CustomerId = request.CustomerId
            });
        }
        catch (Exception ex)
        {
            throw new ApplicationFailureException("Failed to deposit", ex);
        }
    }
}