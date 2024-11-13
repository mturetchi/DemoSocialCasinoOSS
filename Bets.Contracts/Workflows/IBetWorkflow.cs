using Bets.Contracts.DTOs;
using Bets.Contracts.Requests;
using Temporalio.Workflows;

namespace Bets.Contracts.Workflows;

[Workflow]
public interface IBetWorkflow
{
    static string Queue
    {
        get
        {
            return "BET_TASK_QUEUE";
        }
    }

    [WorkflowRun]
    Task<BetDTO> RunAsync(CreateBetRequest request);
}