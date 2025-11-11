using MediatR;

namespace Market.Application.Modules.Identity.Roles.Queries.GetById;

public sealed class GetRoleByIdQuery : IRequest<GetRoleByIdQueryDto>
{
    public int Id { get; init; }
}
