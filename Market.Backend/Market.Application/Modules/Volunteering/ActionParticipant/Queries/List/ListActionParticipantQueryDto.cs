namespace Market.Application.Modules.Volunteering.ActionParticipants.Queries.List;

public sealed class ListActionParticipantsQueryDto
{
    public required int Id { get; init; }
    public string? UserName { get; init; }
    public string? ActionTitle { get; init; }
    public DateTime RegistrationDate { get; init; }
}
