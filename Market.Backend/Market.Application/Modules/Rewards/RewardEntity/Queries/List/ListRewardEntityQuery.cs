using MediatR;

namespace Market.Application.Modules.Rewards.Reward.Queries.List;

public sealed class ListRewardsQuery : BasePagedQuery<ListRewardsQueryDto>
{
    public string? Search { get; init; }          // name/description
    public int? MinPoints { get; init; }          // filter: MinimumPoints >=
    public int? MaxPoints { get; init; }          // filter: MinimumPoints <=
}
