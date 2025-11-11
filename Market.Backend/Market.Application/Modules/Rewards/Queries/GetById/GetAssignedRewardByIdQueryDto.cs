// Market.Application/Modules/Rewards/AssignedRewards/Queries/GetById/GetAssignedRewardByIdQueryDto.cs
namespace Market.Application.Modules.Rewards.AssignedRewards.Queries.GetById;

public sealed class GetAssignedRewardByIdQueryDto
{
    public required int Id { get; init; }
    public required string UserName { get; init; }      // korisnik kome je nagrada dodijeljena
    public required string RewardName { get; init; }    // naziv nagrade
    public required DateTime AssignmentDate { get; init; }
}
