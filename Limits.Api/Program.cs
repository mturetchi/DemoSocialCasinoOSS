using Limits.Contracts.DTOs;
using Limits.Contracts.Requests;

var builder = WebApplication.CreateBuilder(args);

builder
    .Logging
    .AddSimpleConsole()
    .SetMinimumLevel(LogLevel.Information);

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.MapGet("/limits", () => new LimitDTO { BetVolume = 15, LossLimit = 100 });

app.MapPost("/limits", async (UpdateLimitsRequest request) =>  await Task.Delay(10000));

app.Run();
