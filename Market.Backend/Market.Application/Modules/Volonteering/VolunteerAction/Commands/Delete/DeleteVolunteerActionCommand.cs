using MediatR;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Commands.Delete;

public sealed class DeleteVolunteerActionCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
