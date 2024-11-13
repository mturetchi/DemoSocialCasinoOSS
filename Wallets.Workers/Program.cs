using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Temporalio.Extensions.Hosting;
using Wallets.Application;
using Wallets.Application.Interfaces;
using Wallets.Contracts.Workflows;
using Wallets.Infrastructure;
using Wallets.Workers.Activities;
using Wallets.Workers.Deposit;
using Wallets.Workers.Withdrawal;

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

    var temporalioHost = configuration.GetValue<string>("TemporalioHost");

    services.AddHostedTemporalWorker(
        clientTargetHost: temporalioHost,
        clientNamespace: "default",
        taskQueue: IWithdrawalWorkflow.Queue
    )
    .AddScopedActivities<WithdrawalActivity>()
    .AddWorkflow<WithdrawalWorkflow>();

    services.AddHostedTemporalWorker(
        clientTargetHost: temporalioHost,
        clientNamespace: "default",
        taskQueue: IDepositWorkflow.Queue
    )
    .AddScopedActivities<DepositActivity>()
    .AddWorkflow<DepositWorkflow>();
});

var host = builder.Build();

var dbContext = host.Services.GetRequiredService<IWalletsWritableDbContext>();
await dbContext.Database.MigrateAsync();

host.Run();
