using Microsoft.Extensions.DependencyInjection;
using Temporalio.Client;
using Temporalio.Worker;
using Wallets.Application;
using Wallets.Infrastructure;
using Wallets.Workers.Withdrawal;

var serviceCollection = new ServiceCollection();
serviceCollection.AddApplication();
serviceCollection.AddInfrastructure();

var client = await TemporalClient.ConnectAsync(new("localhost:7233"));

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    cts.Cancel();
    eventArgs.Cancel = true;
};

var activities = new WithdrawalActivities();

using var worker = new TemporalWorker(
    client,
    new TemporalWorkerOptions(taskQueue: "WITHDRAWAL_TASK_QUEUE")
        .AddAllActivities(activities)
        .AddWorkflow<WithdrawalWorkflow>()
);

Console.WriteLine("Running worker...");
try
{
    await worker.ExecuteAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Worker cancelled");
}