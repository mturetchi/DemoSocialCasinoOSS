using Temporalio.Workflows;
using Wallets.Contracts.DTOs;
using Wallets.Contracts.Requests;

namespace Wallets.Contracts.Workflows;

[Workflow]
public interface IWithdrawalWorkflow
{
    static string Queue
    {
        get
        {
            return "WITHDRAWAL_TASK_QUEUE";
        }
    }

    [WorkflowRun]
    Task<WalletDTO> RunAsync(WithdrawRequest request);
}
