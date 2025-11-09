using Market.Domain.Common;

namespace Market.Domain.Entities.Rewards;

public class RewardEntity : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public int MinimumPoints { get; set; }

    public IReadOnlyCollection<AssignedRewardEntity> Assignments { get; private set; } = new List<AssignedRewardEntity>();

    public static class Constraints
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;
    }
}
