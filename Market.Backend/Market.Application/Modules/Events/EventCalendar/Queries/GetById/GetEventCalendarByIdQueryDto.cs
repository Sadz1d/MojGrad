namespace Market.Application.Modules.Civic.Events.Queries.GetById;

public sealed class GetEventCalendarByIdQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required DateTime EventDate { get; init; }
    public string? EventType { get; init; }
}
