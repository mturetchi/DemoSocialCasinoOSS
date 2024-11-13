using Bets.Workers.Activities;
using Temporalio.Workflows;

namespace Bets.Workers.Workflows;

[Workflow]
public class BetLimitWorkflow
{
    [WorkflowRun]
    public async Task RunAsync(Guid userId, decimal amount)
    {
        await Workflow.ExecuteActivityAsync(
            (BetLimitActivity activity) => activity.UpdateLimitsAsync(userId, amount),
            new ActivityOptions
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(1),
            }
        );
    }
}
