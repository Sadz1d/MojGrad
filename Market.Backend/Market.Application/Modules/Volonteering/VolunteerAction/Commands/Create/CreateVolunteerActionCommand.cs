using MediatR;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Commands.Create;

public sealed class CreateVolunteerActionCommand : IRequest<int>
{
    public int? VolunteerId { get; init; }          // opcionalno: organizator (User)
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Location { get; init; }
    public required DateTime EventDate { get; init; }
    public required int MaxParticipants { get; init; }
}
