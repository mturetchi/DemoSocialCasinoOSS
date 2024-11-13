using Temporalio.Workflows;
using Wallets.Contracts.DTOs;
using Wallets.Contracts.Requests;

namespace Wallets.Contracts.Workflows;

[Workflow]
public interface IDepositWorkflow
{
    static string Queue
    {
        get
        {
            return "DEPOSIT_TASK_QUEUE";
        }
    }

    [WorkflowRun]
    Task<WalletDTO> RunAsync(DepositRequest request);
}
