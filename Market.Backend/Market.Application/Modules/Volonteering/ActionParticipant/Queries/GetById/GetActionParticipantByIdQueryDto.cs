namespace Market.Application.Modules.Volunteering.ActionParticipants.Queries.GetById;

public sealed class GetActionParticipantByIdQueryDto
{
    public required int Id { get; init; }
    public int? UserId { get; init; }
    public string? UserName { get; init; }
    public int? ActionId { get; init; }
    public string? ActionName { get; init; }
    public DateTime RegistrationDate { get; init; }
}
