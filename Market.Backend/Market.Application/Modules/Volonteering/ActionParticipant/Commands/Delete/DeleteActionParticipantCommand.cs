using MediatR;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Commands.Delete;

public sealed class DeleteActionParticipantCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
