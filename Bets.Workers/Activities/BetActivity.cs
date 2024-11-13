using Bets.Application.Commands.CreateBets;
using Bets.Contracts.DTOs;
using Bets.Contracts.Requests;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using Temporalio.Activities;
using Temporalio.Exceptions;
using Wallets.Contracts.DTOs;
using Wallets.Contracts.Requests;
using Temporalio.Client;

namespace Bets.Workers.Activities;

public class BetActivity
{
    private readonly IMediator _mediator;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ITemporalClient _temporalClient;

    public BetActivity(
        IMediator mediator,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ITemporalClient temporalClient
    )
    {
        _mediator = mediator;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _temporalClient = temporalClient;
    }

    [Activity]
    public async Task<BetDTO> PlaceBetAsync(CreateBetRequest request)
    {
        try
        {
            return await _mediator.Send(new CreateBetCommand
            {
                Amount = request.Amount,
                UserId = request.UserId,
                GameId = request.GameId,
                TransactionId = request.TransactionId
            });
        }
        catch (Exception ex)
        {
            throw new ApplicationFailureException("Failed to withdraw", ex);
        }
    }

    [Activity]
    public async Task<WalletDTO> WithdrawAsync(Guid userId, decimal amount)
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            var data = new WithdrawRequest
            {
                Amount = amount,
                CustomerId = userId,
            };
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{_configuration.GetValue<string>("WalletHost")}/wallets/withdraw", content);
            var resultContent = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WalletDTO>(resultContent);
        }
        catch (Exception ex)
        {
            throw new ApplicationFailureException("Failed to withdraw", ex);
        }
    }
}
