using Market.Domain.Common;

namespace Market.Application.Modules.Identity.UserRoles.Queries.List;

public sealed class ListUserRolesQuery : BasePagedQuery<ListUserRolesDto>
{
    public string? Search { get; init; }
}
