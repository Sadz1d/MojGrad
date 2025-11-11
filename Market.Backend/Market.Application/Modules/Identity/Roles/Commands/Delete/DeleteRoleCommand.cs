using MediatR;

namespace Market.Application.Modules.Identity.Roles.Commands.Delete;

public sealed class DeleteRoleCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
