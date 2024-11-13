namespace Limits.Contracts.Requests;

public class UpdateLimitsRequest
{
    public decimal LossLimit { get; set; }
    public decimal BetVolume { get; set; }
}
