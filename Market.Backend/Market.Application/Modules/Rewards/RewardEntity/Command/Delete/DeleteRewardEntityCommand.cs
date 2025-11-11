using MediatR;

namespace Market.Application.Modules.Rewards.Reward.Commands.Delete;

public sealed class DeleteRewardCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
