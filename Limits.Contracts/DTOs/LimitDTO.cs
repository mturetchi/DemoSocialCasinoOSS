namespace Limits.Contracts.DTOs;

public class LimitDTO
{
    public decimal LossLimit { get; set; } = 100;
    public decimal BetVolume { get; set; } = 15;
}
