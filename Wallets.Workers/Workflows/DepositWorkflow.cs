using Temporalio.Common;
using Temporalio.Exceptions;
using Temporalio.Workflows;
using Wallets.Contracts.DTOs;
using Wallets.Contracts.Requests;
using Wallets.Contracts.Workflows;
using Wallets.Workers.Activities;

namespace Wallets.Workers.Deposit;

[Workflow]
public class DepositWorkflow : IDepositWorkflow
{
    public const string Queue = "DEPOSIT_TASK_QUEUE";

    [WorkflowRun]
    public async Task<WalletDTO> RunAsync(DepositRequest request)
    {
        var retryPolicy = new RetryPolicy
        {
            InitialInterval = TimeSpan.FromSeconds(1),
            MaximumInterval = TimeSpan.FromSeconds(100),
            BackoffCoefficient = 2,
            MaximumAttempts = 3,
            NonRetryableErrorTypes = ["InvalidAccountException", "InsufficientFundsException"]
        };

        try
        {
            return await Workflow.ExecuteActivityAsync(
                (DepositActivity activity) => activity.DepositAsync(request),
                new ActivityOptions
                {
                    StartToCloseTimeout = TimeSpan.FromMinutes(5),
                    RetryPolicy = retryPolicy
                }
            );
        }
        catch (ApplicationFailureException ex) when (ex.ErrorType == "InsufficientFundsException")
        {
            throw new ApplicationFailureException("Deposit failed due to insufficient funds.", ex);
        }
    }
}
