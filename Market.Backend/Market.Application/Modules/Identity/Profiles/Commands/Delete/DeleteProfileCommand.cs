using MediatR;

namespace Market.Application.Modules.Identity.Profiles.Commands.Delete;

public sealed class DeleteProfileCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
