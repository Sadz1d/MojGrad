using Market.Domain.Common;

namespace Market.Domain.Entities.Identity;

public sealed class RoleEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<UserRoleEntity> UserRoles { get; private set; } = new List<UserRoleEntity>();
}
