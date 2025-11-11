using MediatR;

namespace Market.Application.Modules.Rewards.AssignedRewards.Commands.Create;

public sealed class CreateAssignedRewardCommand : IRequest<int>
{
    public required int UserId { get; init; }
    public required int RewardId { get; init; }
    public DateTime? AssignmentDate { get; init; } // ako null -> stavljamo UtcNow
}
