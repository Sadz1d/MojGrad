// Market.Application/Modules/Rewards/AssignedRewards/Queries/List/ListAssignedRewardsQueryDto.cs
namespace Market.Application.Modules.Rewards.AssignedRewards.Queries.List;

public sealed class ListAssignedRewardsQueryDto
{
    public required int Id { get; init; }
    public required string UserName { get; init; }      // MarketUser (First + Last)
    public required string RewardName { get; init; }    // Reward.Title (ili Name)
    public required DateTime AssignmentDate { get; init; }
}
