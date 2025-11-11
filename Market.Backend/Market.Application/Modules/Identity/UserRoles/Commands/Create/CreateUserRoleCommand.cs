using MediatR;

namespace Market.Application.Modules.Identity.UserRoles.Commands.Create;

public sealed class CreateUserRoleCommand : IRequest<int>
{
    public required int UserId { get; init; }
    public required int RoleId { get; init; }
}
