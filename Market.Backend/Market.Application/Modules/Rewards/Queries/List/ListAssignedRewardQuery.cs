// Market.Application/Modules/Rewards/AssignedRewards/Queries/List/ListAssignedRewardsQuery.cs
using Market.Application.Modules.Rewards.AssignedReward.Queries.List;
using MediatR;

namespace Market.Application.Modules.Rewards.AssignedRewards.Queries.List;

public sealed class ListAssignedRewardsQuery : BasePagedQuery<ListAssignedRewardsQueryDto>
{
    public string? Search { get; init; }     // user name / reward name
    public int? UserId { get; init; }        // filter by user
    public int? RewardId { get; init; }      // filter by reward
}
