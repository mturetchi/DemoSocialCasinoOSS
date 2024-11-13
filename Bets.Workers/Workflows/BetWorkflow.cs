using Bets.Contracts.DTOs;
using Bets.Contracts.Requests;
using Bets.Contracts.Workflows;
using Bets.Workers.Activities;
using Temporalio.Common;
using Temporalio.Workflows;

namespace Bets.Workers.Workflows;

[Workflow]
public class BetWorkflow : IBetWorkflow
{
    public const string Queue = "BET_TASK_QUEUE";

    [WorkflowRun]
    public async Task<BetDTO> RunAsync(CreateBetRequest request)
    {
        var retryPolicy = new RetryPolicy
        {
            InitialInterval = TimeSpan.FromSeconds(1),
            MaximumInterval = TimeSpan.FromSeconds(100),
            BackoffCoefficient = 2,
            MaximumAttempts = 3,
            NonRetryableErrorTypes = ["InvalidAccountException", "InsufficientFundsException"]
        };

        var limits = await Workflow.ExecuteActivityAsync(
            (BetLimitActivity activity) => activity.GetLimitsAsync(request.UserId, request.Amount),
            new ActivityOptions
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(5),
                RetryPolicy = retryPolicy
            }
        );

        // Limits validation
        // ...
        // -----------------

        var risks = await Workflow.ExecuteActivityAsync(
            (BetRiskActivity activity) => activity.GetRisksAsync(request.UserId, request.Amount),
            new ActivityOptions
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(5),
                RetryPolicy = retryPolicy
            }
        );

        // Risks validation
        // ....
        // ----------------
        var withdrawResult = await Workflow.ExecuteActivityAsync(
            (BetActivity activity) => activity.WithdrawAsync(request.UserId, request.Amount),
            new ActivityOptions
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(5),
                RetryPolicy = retryPolicy
            }
        );

        var bet = await Workflow.ExecuteActivityAsync(
            (BetActivity activity) => activity.PlaceBetAsync(request),
            new ActivityOptions
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(5),
                RetryPolicy = retryPolicy
            }
        );

        bet.WalletAmount = withdrawResult.Amount;

        await Workflow.StartChildWorkflowAsync(
            (BetLimitWorkflow wf) => wf.RunAsync(request.UserId, request.Amount),
            new ChildWorkflowOptions
            {
                Id = $"bet-limit-workflow-{Guid.NewGuid()}",
                ParentClosePolicy = ParentClosePolicy.Abandon
            });

        await Workflow.StartChildWorkflowAsync(
            (BetRiskWorkflow wf) => wf.RunAsync(request.UserId, request.Amount),
            new ChildWorkflowOptions
            {
                Id = $"bet-risk-workflow-{Guid.NewGuid()}",
                ParentClosePolicy = ParentClosePolicy.Abandon
            });

        return bet;
    }
}