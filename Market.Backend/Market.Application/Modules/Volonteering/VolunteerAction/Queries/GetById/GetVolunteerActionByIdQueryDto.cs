namespace Market.Application.Modules.Volunteering.VolunteerActions.Queries.GetById;

public sealed class GetVolunteerActionByIdQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Location { get; init; }
    public required DateTime EventDate { get; init; }
    public required int MaxParticipants { get; init; }

    public int? OrganizerId { get; init; }
    public string? OrganizerName { get; init; }

    public int ParticipantsCount { get; init; }
    public int FreeSlots => MaxParticipants - ParticipantsCount;
}
