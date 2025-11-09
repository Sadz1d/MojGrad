using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Rewards;

public class AssignedRewardEntity : BaseEntity
{
    public int UserId { get; set; }
    public int RewardId { get; set; }
    public DateTime AssignmentDate { get; set; }

    public MarketUserEntity User { get; set; } = default!;
    public RewardEntity Reward { get; set; } = default!;
}
