using Market.Application.Modules.Identity.Roles.Queries.List;
using MediatR;

namespace Market.Application.Modules.Identity.Roles.Queries.List;

public sealed class ListRolesQuery : IRequest<IEnumerable<ListRolesQueryDto>>
{
}
