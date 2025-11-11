using MediatR;

namespace Market.Application.Modules.Identity.UserRoles.Queries.GetById;

public sealed class GetUserRoleByIdQuery : IRequest<GetUserRoleByIdDto>
{
    public int Id { get; init; }
}
