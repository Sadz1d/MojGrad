namespace Market.Application.Modules.Rewards.Reward.Queries.GetById;

public sealed class GetRewardByIdQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required int MinimumPoints { get; init; }
    public int AssignmentsCount { get; init; }
}
