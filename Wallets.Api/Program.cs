using Temporalio.Client;
using Wallets.Contracts.Requests;
using Wallets.Contracts.Workflows;

var builder = WebApplication.CreateBuilder(args);

builder
    .Logging
    .AddSimpleConsole()
    .SetMinimumLevel(LogLevel.Information);

builder.Configuration.AddEnvironmentVariables();

var configuration = builder.Configuration;

var temporalioHost = configuration.GetValue<string>("TemporalioHost");

builder.Services.AddSingleton((services) =>
    TemporalClient.ConnectAsync(new()
    {
        TargetHost = temporalioHost,
        LoggerFactory = services.GetRequiredService<ILoggerFactory>(),
    }));

var app = builder.Build();

app.MapPost("/wallets/deposit", async (Task<TemporalClient> clientTask, DepositRequest request) =>
{
    var client = await clientTask;
    return await client.ExecuteWorkflowAsync(
        (IDepositWorkflow wf) => wf.RunAsync(request),
        new(id: $"deposit-workflow-{Guid.NewGuid()}", taskQueue: IDepositWorkflow.Queue));
});

app.MapPost("/wallets/withdraw", async (Task<TemporalClient> clientTask, WithdrawRequest request) =>
{
    var client = await clientTask;
    return await client.ExecuteWorkflowAsync(
        (IWithdrawalWorkflow wf) => wf.RunAsync(request),
        new(id: $"withdraw-workflow-{Guid.NewGuid()}", taskQueue: IWithdrawalWorkflow.Queue));
});

app.Run();
