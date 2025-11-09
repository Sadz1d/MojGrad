using Market.Domain.Common;

namespace Market.Domain.Entities.Identity;

public sealed class UserRoleEntity : BaseEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    public MarketUserEntity User { get; set; } = null!;
    public RoleEntity Role { get; set; } = null!;
}
