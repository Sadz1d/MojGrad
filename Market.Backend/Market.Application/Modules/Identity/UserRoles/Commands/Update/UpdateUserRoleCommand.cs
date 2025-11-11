using MediatR;

namespace Market.Application.Modules.Identity.UserRoles.Commands.Update;

public sealed record UpdateUserRoleCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int RoleId { get; init; }
}
