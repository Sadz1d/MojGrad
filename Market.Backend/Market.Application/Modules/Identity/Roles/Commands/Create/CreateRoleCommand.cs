using MediatR;

namespace Market.Application.Modules.Identity.Roles.Commands.Create;

public sealed class CreateRoleCommand : IRequest<int>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
