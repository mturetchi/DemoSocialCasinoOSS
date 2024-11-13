using Bets.Contracts.Requests;
using Bets.Contracts.Workflows;
using Temporalio.Client;

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

app.MapPost("/bets", async (Task<TemporalClient> clientTask, CreateBetRequest request) =>
{
    var client = await clientTask;
    return await client.ExecuteWorkflowAsync(
        (IBetWorkflow wf) => wf.RunAsync(request),
        new(id: $"bet-workflow-{Guid.NewGuid()}", taskQueue: IBetWorkflow.Queue));
});

app.Run();
