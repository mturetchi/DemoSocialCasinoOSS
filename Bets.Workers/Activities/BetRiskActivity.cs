using Microsoft.Extensions.Configuration;
using Risks.Contracts.Requests;
using System.Text.Json;
using System.Text;
using Temporalio.Activities;
using Risks.Contracts.DTOs;
using Temporalio.Exceptions;

namespace Bets.Workers.Activities;

public class BetRiskActivity
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public BetRiskActivity(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration    
    )
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [Activity]
    public async Task UpdateRisksAsync(Guid userId, decimal amount)
    {
        using var client = _httpClientFactory.CreateClient();
        var data = new UpdateRiskRequest
        {
            WinVolume = amount,
        };
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await client.PostAsync($"{_configuration.GetValue<string>("RiskHost")}/risks", content);
        result.EnsureSuccessStatusCode();
    }

    [Activity]
    public async Task<RiskDTO> GetRisksAsync(Guid userId, decimal amount)
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($"{_configuration.GetValue<string>("RiskHost")}/risks");
            var resultContent = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RiskDTO>(resultContent);
        }
        catch (Exception ex)
        {
            throw new ApplicationFailureException("Failed to get risks", ex);
        }
    }
}
