using Bets.Workers.Activities;
using Temporalio.Workflows;

namespace Bets.Workers.Workflows;

[Workflow]
public class BetRiskWorkflow
{
    [WorkflowRun]
    public async Task RunAsync(Guid userId, decimal amount)
    {
        await Workflow.ExecuteActivityAsync(
            (BetRiskActivity activity) => activity.UpdateRisksAsync(userId, amount),
            new ActivityOptions
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(1),
            }
        );
    }
}
