using Risks.Contracts.DTOs;
using Risks.Contracts.Requests;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/risks", () => new RiskDTO { WinVolume = 1000 });

app.MapPost("/risks", async (UpdateRiskRequest request) => await Task.Delay(10000));

app.Run();
