namespace Market.Application.Modules.Volunteering.VolunteerActions.Queries.List;

public sealed class ListVolunteerActionsQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Location { get; init; }
    public required DateTime EventDate { get; init; }
    public required int MaxParticipants { get; init; }
    public int ParticipantsCount { get; init; }
    public int FreeSlots => MaxParticipants - ParticipantsCount;
}
