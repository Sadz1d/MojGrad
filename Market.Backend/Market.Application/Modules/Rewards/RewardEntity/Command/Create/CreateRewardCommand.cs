using MediatR;

namespace Market.Application.Modules.Rewards.Reward.Commands.Create;

public sealed class CreateRewardCommand : IRequest<int>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required int MinimumPoints { get; init; }
}
