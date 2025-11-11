using MediatR;

namespace Market.Application.Modules.Identity.UserRoles.Commands.Delete;

public sealed record DeleteUserRoleCommand(int Id) : IRequest<Unit>;
