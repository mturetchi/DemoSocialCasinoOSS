using Limits.Contracts.DTOs;
using Limits.Contracts.Requests;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using Temporalio.Activities;
using Temporalio.Exceptions;

namespace Bets.Workers.Activities;

public class BetLimitActivity
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public BetLimitActivity(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    )
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }


    [Activity]
    public async Task UpdateLimitsAsync(Guid userId, decimal amount)
    {
        using var client = _httpClientFactory.CreateClient();
        var data = new UpdateLimitsRequest
        {
            BetVolume = amount,
            LossLimit = 100
        };
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await client.PostAsync($"{_configuration.GetValue<string>("LimitHost")}/limits", content);
        result.EnsureSuccessStatusCode();
    }

    [Activity]
    public async Task<LimitDTO> GetLimitsAsync(Guid userId, decimal amount)
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($"{_configuration.GetValue<string>("LimitHost")}/limits");
            var resultContent = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LimitDTO>(resultContent);
        }
        catch (Exception ex)
        {
            throw new ApplicationFailureException("Failed to get limits", ex);
        }
    }
}
