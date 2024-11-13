using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Bets.Application;
using Bets.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Temporalio.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Bets.Application.Interfaces;
using Bets.Contracts.Workflows;
using Bets.Workers.Activities;
using Bets.Workers.Workflows;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(configurationBuilder =>
    configurationBuilder
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
);

builder.ConfigureLogging(loggingBuilder =>
    loggingBuilder
        .AddSimpleConsole()
        .SetMinimumLevel(LogLevel.Information)
);

builder.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration;
    services.AddApplication();
    services.AddInfrastructure(configuration);
    services.AddHttpClient();

    var temporalioHost = configuration.GetValue<string>("TemporalioHost");
    services.AddTemporalClient(
        clientTargetHost: temporalioHost,
        clientNamespace: "default"
    );
    services.AddHostedTemporalWorker(
        clientTargetHost: temporalioHost,
        clientNamespace: "default",
        taskQueue: IBetWorkflow.Queue
    )
    .AddScopedActivities<BetActivity>()
    .AddScopedActivities<BetLimitActivity>()
    .AddScopedActivities<BetRiskActivity>()
    .AddWorkflow<BetLimitWorkflow>()
    .AddWorkflow<BetRiskWorkflow>()
    .AddWorkflow<BetWorkflow>();
});

var host = builder.Build();

var dbContext = host.Services.GetRequiredService<IBetsWritableDbContext>();
await dbContext.Database.MigrateAsync();

host.Run();
